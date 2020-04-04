namespace Mayfair.Core.Code.GameScene
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Mayfair.Core.Code.GameData.Messages;
    using Mayfair.Core.Code.LoadingProcess;
    using Mayfair.Core.Code.LoadingProcess.Enums;
    using Mayfair.Core.Code.LoadingProcess.Messages;
    using Mayfair.Core.Code.Resources;
    using Mayfair.Core.Code.Resources.Enums;
    using Mayfair.Core.Code.Resources.Loader;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Helpers.Regexp;
    using Messages;
    using Prateek.NoticeFramework.Notices.Core;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;
    using Utils.Debug;

    public sealed class GameSceneDaemonCore : ResourceDependentDaemonCore<GameSceneDaemonCore, GameSceneDaemonBranch>
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
        private TaskLoadingNotice taskNotice;
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
        #endregion

        #region Unity Methods
        protected override void Update()
        {
            base.Update();

            UpdateLoadingTask();
        }
        #endregion

        #region Service
        protected override void OnAwake() { }
        #endregion

        #region Messaging
        public override void NoticeReceived() { }

        protected override void SetupNoticeReceiverCallback()
        {
            base.SetupNoticeReceiverCallback();

            this.NoticeReceiver.AddCallback<GameSessionOpen>(OnGameSessionOpen);
            this.NoticeReceiver.AddCallback<GameSessionClose>(OnGameSessionClose);
            this.NoticeReceiver.AddCallback<GameLoadingGameplayNotice>(OnGameLoadingGameplay);
            this.NoticeReceiver.AddCallback<GameLoadingRestartNotice>(OnGameLoadingRestart);

            this.NoticeReceiver.AddCallback<LoadSceneRequest<LoadSceneResponse>>(OnLoadSceneRequestReceived);
            this.NoticeReceiver.AddCallback<UnloadSceneRequest<UnloadSceneResponse>>(OnUnloadSceneRequestReceived);
        }
        #endregion

        #region Class Methods
        public void Add(SceneReference scene)
        {
            matches.Clear();
            if (RegexHelper.TryFetchingMatches(scene.Loader.Location, RegexHelper.AddressTag, matches))
            {
                string context = matches[CONTEXT_TAG];
                AvailableScenes container;
                if (!availableScenes.TryGetValue(context, out container))
                {
                    container.scenes = new List<SceneReference>();
                    availableScenes.Add(context, container);
                }

                container.scenes.Add(scene);

                if (matches.Count > 1)
                {
                    LightingSceneType[] values = (LightingSceneType[]) Enum.GetValues(typeof(LightingSceneType));
                    string[] names = Enum.GetNames(typeof(LightingSceneType));
                    for (int n = 0; n < names.Length; n++)
                    {
                        LightingSceneType value = values[n];
                        string name = names[n];
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

                this.PlaceLightingSceneInLast(ref container);

                this.availableScenes[context] = container;

                SessionDebugAvailable notice = Notice.Create<SessionDebugAvailable>();
                notice.Init(context);
                this.NoticeReceiver.Send(notice);
            }
        }

        private void SendStatusMessage(LoadingTrackingStatus status, float progress = 1)
        {
            if (taskNotice == null)
            {
                taskNotice = Notice.Create<TaskLoadingNotice>();
            }

            taskNotice.trackerState = new LoadingTaskTracker(GetType(), status)
            {
                StepProgress = progress
            };

            NoticeReceiver.Send(taskNotice);
        }

        private void UpdateLoadingTask()
        {
            if (serviceState == ServiceState.Idle
             || taskNotice == null
             || activeContainer.scenes == null)
            {
                return;
            }

            float progress = 0f;
            bool isDone = true;

            //A bit clunky, if unloadCompletedActions is < 0, it means we're loading
            if (serviceState == ServiceState.Loading)
            {
                float step = 1f / activeContainer.scenes.Count;
                foreach (SceneReference scene in activeContainer.scenes)
                {
                    progress += step * scene.Loader.PercentComplete;
                    isDone = isDone && scene.Loader.IsDone;
                }
            }
            else if (serviceState == ServiceState.Unloading)
            {
                float step = 1f / activeContainer.scenes.Count;
                progress = completedActions / (float) activeContainer.scenes.Count;
                foreach (SceneReference scene in activeContainer.scenes)
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
                taskNotice = null;
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
            foreach (AvailableScenes container in activeContainers)
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

        private void OnGameLoadingGameplay(GameLoadingGameplayNotice notice)
        {
            completedActions = Consts.RESET;

            if (!availableScenes.TryGetValue(activeSession.SessionContext, out AvailableScenes containerRequested))
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

        private void OnGameLoadingRestart(GameLoadingRestartNotice notice)
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
                for (int i = 0; i < container.scenes.Count - 1; i++)
                {
                    SceneReference sceneRef = container.scenes[i];
                    if (sceneRef == container.lightingScene)
                    {
                        container.scenes[i] = container.scenes[container.scenes.Count-1];
                        container.scenes[container.scenes.Count-1] = sceneRef;
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
                SceneRequest request = containersRequestedQueue.Peek();
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

            foreach (SceneReference scene in activeContainer.scenes)
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
                foreach (SceneReference scene in activeContainer.scenes)
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
            if (!availableScenes.TryGetValue(request.Scene, out AvailableScenes containerRequested))
            {
                NoticeReceiver.Send(request.GetResponse());

                Debug.Assert(false, $"Requested scene {request.Scene} does not exist or does not have an address.");
                return;
            }
            
            containersRequestedQueue.Enqueue(new SceneRequest
            {
                container = containerRequested,
                state = ServiceState.Loading,
                onFinished = sceneReference =>
                {
                    LoadSceneRequest<LoadSceneResponse> sceneRequest = request;
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
            LoadSceneResponse response = request.GetResponse();
            response.SceneReference = sceneReference;
            NoticeReceiver.Send(response);
        }

        private void OnUnloadSceneRequestReceived(UnloadSceneRequest<UnloadSceneResponse> request)
        {
            DebugTools.Log($"Game scene request to unload '{request.Scene}' received", DebugTools.LogLevel.LowPriority);
            if (!availableScenes.TryGetValue(request.Scene, out AvailableScenes containerRequested))
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
                    UnloadSceneRequest<UnloadSceneResponse> sceneRequest = request;
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
            UnloadSceneResponse response = request.GetResponse();
            NoticeReceiver.Send(response);
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

        #region Nested type: AvailableScenes
        private struct SceneRequest
        {
            public AvailableScenes container;
            public ServiceState state;
            public Action<SceneReference> onFinished;

            public void OnFinished(IAbstractResourceReference reference)
            {
                onFinished(reference as SceneReference);
            }
        }
        #endregion
    }
}
