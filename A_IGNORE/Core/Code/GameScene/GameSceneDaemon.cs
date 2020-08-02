namespace Mayfair.Core.Code.GameScene
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Mayfair.Core.Code.GameData.Messages;
    using Mayfair.Core.Code.GameScene.Messages;
    using Mayfair.Core.Code.LoadingProcess;
    using Mayfair.Core.Code.LoadingProcess.Enums;
    using Mayfair.Core.Code.LoadingProcess.Messages;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Debug;
    using Mayfair.Core.Code.Utils.Helpers.Regexp;
    using Prateek.A_TODO.Runtime.AppContentFramework.Daemons;
    using Prateek.A_TODO.Runtime.AppContentFramework.Enums;
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader.Interfaces;
    using Prateek.A_TODO.Runtime.AppContentUnityIntegration;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.TickableFramework.Enums;
    using UnityEngine.SceneManagement;

    public sealed class GameSceneDaemon : ContentAccessDaemon<GameSceneDaemon, GameSceneServant>
    {
        #region LightingSceneType enum
        public enum LightingSceneType
        {
            Default,

            Visual,
            Lighting
        }
        #endregion

        #region ServiceState enum
        private enum ServiceState
        {
            Idle,
            Loading,
            Unloading
        }
        #endregion

        #region Static and Constants
        private const int CONTEXT_TAG = 0;
        private const int USAGE_TAG = 1;
        #endregion

        #region Fields
        private ServiceState serviceState = ServiceState.Idle;
        private TaskLoadingCommand taskCommand;
        private GameSessionOpen activeSession;
        private AvailableScenes activeContainer;
        private int completedActions = Consts.INDEX_NONE;
        private List<string> matches = new List<string>();
        private Dictionary<string, AvailableScenes> availableScenes = new Dictionary<string, AvailableScenes>();
        private readonly Queue<SceneRequest> containersRequestedQueue = new Queue<SceneRequest>();
        private readonly List<AvailableScenes> activeContainers = new List<AvailableScenes>();
        private SceneRequest currentRequest;
        #endregion

        #region Properties
        protected override ServiceProviderUsageRuleType ServiceProviderUsageRule
        {
            get { return ServiceProviderUsageRuleType.UseFirstValid; }
        }

        public override TickableSetup TickableSetup
        {
            get { return TickableSetup.UpdateBegin; }
        }
        #endregion

        #region Register/Unregister
        #region Service
        protected override void OnAwake() { }
        #endregion
        #endregion

        #region Class Methods
        public override void Tick(TickableFrame tickableFrame, float seconds, float unscaledSeconds)
        {
            base.Tick(tickableFrame, seconds, unscaledSeconds);

            UpdateLoadingTask();
        }

        public void Add(SceneReference scene)
        {
            matches.Clear();
            if (RegexHelper.TryFetchingMatches(scene.Loader.Location, RegexHelper.AddressTag, matches))
            {
                var             context = matches[CONTEXT_TAG];
                AvailableScenes container;
                if (!availableScenes.TryGetValue(context, out container))
                {
                    container.scenes = new List<SceneReference>();
                    availableScenes.Add(context, container);
                }

                container.scenes.Add(scene);

                if (matches.Count > 1)
                {
                    var values = (LightingSceneType[]) Enum.GetValues(typeof(LightingSceneType));
                    var names  = Enum.GetNames(typeof(LightingSceneType));
                    for (var n = 0; n < names.Length; n++)
                    {
                        var value = values[n];
                        var name  = names[n];
                        if (matches[USAGE_TAG] == name && value > container.lightingType)
                        {
                            container.lightingType = value;
                            container.lightingScene = scene;
                            break;
                        }
                    }
                }
                else if (container.lightingType == LightingSceneType.Default)
                {
                    container.lightingScene = scene;
                }

                PlaceLightingSceneInLast(ref container);

                availableScenes[context] = container;

                var notice = Command.Create<SessionDebugAvailable>();
                notice.Init(context);
                CommandReceiver.Send(notice);
            }
        }

        private void SendStatusMessage(LoadingTrackingStatus status, float progress = 1)
        {
            if (taskCommand == null)
            {
                taskCommand = Command.Create<TaskLoadingCommand>();
            }

            taskCommand.trackerState = new LoadingTaskTracker(GetType(), status)
            {
                StepProgress = progress
            };

            CommandReceiver.Send(taskCommand);
        }

        private void UpdateLoadingTask()
        {
            if (serviceState == ServiceState.Idle
             || taskCommand == null
             || activeContainer.scenes == null)
            {
                return;
            }

            var progress = 0f;
            var isDone   = true;

            //A bit clunky, if unloadCompletedActions is < 0, it means we're loading
            if (serviceState == ServiceState.Loading)
            {
                var step = 1f / activeContainer.scenes.Count;
                foreach (var scene in activeContainer.scenes)
                {
                    progress += step * scene.Loader.PercentComplete;
                    isDone = isDone && scene.Loader.IsDone;
                }
            }
            else if (serviceState == ServiceState.Unloading)
            {
                var step = 1f / activeContainer.scenes.Count;
                progress = completedActions / (float) activeContainer.scenes.Count;
                foreach (var scene in activeContainer.scenes)
                {
                    progress += step * scene.Loader.PercentComplete;
                    isDone = isDone && scene.Loader.IsDone;
                }
            }

            if (!isDone)
            {
                SendStatusMessage(LoadingTrackingStatus.LoadingPrerequisite, progress);
            }
            else if (isDone && completedActions == activeContainer.scenes.Count)
            {
                SendStatusMessage(LoadingTrackingStatus.Finished);
                taskCommand = null;
                serviceState = ServiceState.Idle;

                HandlePostponedRequests();
            }
        }

        private void OnGameSessionOpen(GameSessionOpen notice)
        {
            activeSession = notice;
        }

        private void OnGameSessionClose(GameSessionClose notice)
        {
            foreach (var container in activeContainers)
            {
                containersRequestedQueue.Enqueue(new SceneRequest
                {
                    container = container,
                    state = ServiceState.Unloading,
                    onFinished = UnloadAssetCompleted
                });
            }

            if (serviceState != ServiceState.Idle)
            {
                return;
            }

            StartUnloadSceneProcess();
        }

        private void OnGameLoadingGameplay(GameLoadingGameplayCommand command)
        {
            completedActions = Consts.RESET;

            if (!availableScenes.TryGetValue(activeSession.SessionContext, out var containerRequested))
            {
                Debug.Assert(false, $"Requested scene {activeSession.SessionContext} does not exist or does not have an address.");
                return;
            }

            containersRequestedQueue.Enqueue(new SceneRequest
            {
                container = containerRequested,
                state = ServiceState.Loading,
                onFinished = LoadAssetCompleted
            });

            if (serviceState != ServiceState.Idle)
            {
                return;
            }

            StartLoadSceneProcess();
        }

        private void OnGameLoadingRestart(GameLoadingRestartCommand command)
        {
            SendStatusMessage(LoadingTrackingStatus.StartedLoading);
        }

        private void LoadAssetCompleted(SceneReference container)
        {
            if (container.Loader.Status != AsyncStatus.Loaded)
            {
                return;
            }

            container.Resource.Activate();
            if (activeContainer.scenes.Count == 1
             || activeContainer.lightingScene != null && activeContainer.lightingScene.Loader == container.Loader)
            {
                SceneManager.SetActiveScene(container.Resource.Scene);
            }

            RefreshLoadingStatus();
        }

        private void UnloadAssetCompleted(SceneReference container)
        {
            RefreshLoadingStatus();
        }

        private void RefreshLoadingStatus()
        {
            completedActions++;
        }

        private void PlaceLightingSceneInLast(ref AvailableScenes container)
        {
            // place lighting scene load in last position
            if (container.lightingScene != null && container.scenes.Count > 1)
            {
                for (var i = 0; i < container.scenes.Count - 1; i++)
                {
                    var sceneRef = container.scenes[i];
                    if (sceneRef == container.lightingScene)
                    {
                        container.scenes[i] = container.scenes[container.scenes.Count - 1];
                        container.scenes[container.scenes.Count - 1] = sceneRef;
                        break;
                    }
                }
            }
        }

        private void HandlePostponedRequests()
        {
            if (containersRequestedQueue.Count > 0)
            {
                DebugTools.Log("Handle postponed scene requests", DebugTools.LogLevel.LowPriority);
                var request = containersRequestedQueue.Peek();
                if (request.state == ServiceState.Loading)
                {
                    StartLoadSceneProcess();
                }
                else if (request.state == ServiceState.Unloading)
                {
                    StartUnloadSceneProcess();
                }
            }
        }

        private void StartLoadSceneProcess()
        {
            currentRequest = containersRequestedQueue.Dequeue();
            activeContainer = currentRequest.container;

            SendStatusMessage(LoadingTrackingStatus.StartedLoading);

            completedActions = Consts.RESET;

            serviceState = ServiceState.Loading;

            foreach (var scene in activeContainer.scenes)
            {
                scene.LoadCompleted = currentRequest.OnFinished;
                scene.LoadAsync();
            }

            activeContainers.Add(activeContainer);
        }

        private void StartUnloadSceneProcess()
        {
            currentRequest = containersRequestedQueue.Dequeue();
            activeContainer = currentRequest.container;

            SendStatusMessage(LoadingTrackingStatus.StartedLoading);

            completedActions = Consts.RESET;

            serviceState = ServiceState.Unloading;

            if (activeContainer.scenes != null)
            {
                foreach (var scene in activeContainer.scenes)
                {
                    scene.LoadCompleted = currentRequest.OnFinished;
                    scene.UnloadAsync();
                }
            }

            activeContainers.Remove(activeContainer);
        }

        private void OnLoadSceneRequestReceived(LoadSceneRequest<LoadSceneResponse> request)
        {
            DebugTools.Log($"Game scene request to load '{request.Scene}' received", DebugTools.LogLevel.LowPriority);
            if (!availableScenes.TryGetValue(request.Scene, out var containerRequested))
            {
                CommandReceiver.Send(request.GetResponse());

                Debug.Assert(false, $"Requested scene {request.Scene} does not exist or does not have an address.");
                return;
            }

            containersRequestedQueue.Enqueue(new SceneRequest
            {
                container = containerRequested,
                state = ServiceState.Loading,
                onFinished = sceneReference =>
                {
                    var sceneRequest = request;
                    LoadSceneCompleted(sceneReference, sceneRequest);
                }
            });

            if (serviceState != ServiceState.Idle)
            {
                return;
            }

            StartLoadSceneProcess();
        }

        private void LoadSceneCompleted(SceneReference sceneReference, LoadSceneRequest<LoadSceneResponse> request)
        {
            RefreshLoadingStatus();
            var response = request.GetResponse();
            response.SceneReference = sceneReference;
            CommandReceiver.Send(response);
        }

        private void OnUnloadSceneRequestReceived(UnloadSceneRequest<UnloadSceneResponse> request)
        {
            DebugTools.Log($"Game scene request to unload '{request.Scene}' received", DebugTools.LogLevel.LowPriority);
            if (!availableScenes.TryGetValue(request.Scene, out var containerRequested))
            {
                Debug.Assert(false, $"Requested scene {request.Scene} does not exist or does not have an address.");
                return;
            }

            containersRequestedQueue.Enqueue(new SceneRequest
            {
                container = containerRequested,
                state = ServiceState.Unloading,
                onFinished = sceneReference =>
                {
                    var sceneRequest = request;
                    UnloadSceneCompleted(sceneReference, sceneRequest);
                }
            });

            if (serviceState != ServiceState.Idle)
            {
                return;
            }

            StartUnloadSceneProcess();
        }

        private void UnloadSceneCompleted(SceneReference sceneReference, UnloadSceneRequest<UnloadSceneResponse> request)
        {
            RefreshLoadingStatus();
            var response = request.GetResponse();
            CommandReceiver.Send(response);
        }
        #endregion

        #region Nested type: AvailableScenes
        private struct AvailableScenes
        {
            public List<SceneReference> scenes;
            public LightingSceneType lightingType;
            public SceneReference lightingScene;
        }
        #endregion

        #region Nested type: SceneRequest
        private struct SceneRequest
        {
            public AvailableScenes container;
            public ServiceState state;
            public Action<SceneReference> onFinished;

            public void OnFinished(IContentHandle reference)
            {
                onFinished(reference as SceneReference);
            }
        }
        #endregion

        #region Messaging
        public override void CommandReceived() { }

        protected override void SetupCommandReceiverCallback()
        {
            base.SetupCommandReceiverCallback();

            CommandReceiver.AddCallback<GameSessionOpen>(OnGameSessionOpen);
            CommandReceiver.AddCallback<GameSessionClose>(OnGameSessionClose);
            CommandReceiver.AddCallback<GameLoadingGameplayCommand>(OnGameLoadingGameplay);
            CommandReceiver.AddCallback<GameLoadingRestartCommand>(OnGameLoadingRestart);

            CommandReceiver.AddCallback<LoadSceneRequest<LoadSceneResponse>>(OnLoadSceneRequestReceived);
            CommandReceiver.AddCallback<UnloadSceneRequest<UnloadSceneResponse>>(OnUnloadSceneRequestReceived);
        }
        #endregion
    }
}
