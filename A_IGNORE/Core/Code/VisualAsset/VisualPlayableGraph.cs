namespace Mayfair.Core.Code.VisualAsset
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Mayfair.Core.Code.Animation;
    using Mayfair.Core.Code.Animation.Enums;
    using Mayfair.Core.Code.Animation.Extensions;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Types;
    using UnityEngine;
    using UnityEngine.Animations;
    using UnityEngine.Assertions;
    using UnityEngine.Playables;

    public class VisualPlayableGraph : MonoBehaviour
    {
        private const float ZERO_WEIGHT = 0;
        private const float FULL_WEIGHT = 1;
        private const int ONE_INPUT = 1;
        private const int TWO_INPUT = 2;
        private const int FIRST_OUTPUT = 0;
        private const int FIRST_INPUT = 0;
        private const int SECOND_INPUT = 1;

        #region Settings
        [SerializeField]
        private List<AnimationClip> defaultFullBodyClips = new List<AnimationClip>();

        [SerializeField]
        private List<AnimationClip> defaultAdditiveClips = new List<AnimationClip>();
        #endregion

        #region Fields
        private RuntimeAnimatorController runtimeController;

        private List<AnimationClip> fullBodyClips = new List<AnimationClip>();
        private List<AnimationClip> additiveClips = new List<AnimationClip>();

        public bool needGraphRebuild = false;

        private PlayableGraph mainGraph;
        private AnimationPlayableOutput output;
        private AnimatorControllerPlayable controller;
        private MixerState mixerState = MixerState.Nothing;
        private MixerState nextMixerState = MixerState.Nothing;

        private AnimationMixerPlayable fullBodyMixer;
        private AnimationMixerPlayable fullBodySelector;
        private List<AnimationClipPlayable> fullBodyPlayables = new List<AnimationClipPlayable>();

        private AnimationLayerMixerPlayable additiveMixer;
        private List<AnimationClipPlayable> additivePlayables = new List<AnimationClipPlayable>();

        private DefaultPoseJobHandler poseHandler = new DefaultPoseJobHandler();

        private float fullBodyBlendTime = 0;
        private float fullBodyClipDuration = 0;
        private float fullBodyBlendDuration = 0;
        private int fullBodyActiveIndex = -1;
        #endregion

        #region Properties
        public AnimatorControllerPlayable Controller
        {
            get { return controller; }
        }

        public RuntimeAnimatorController RuntimeController
        {
            set { runtimeController = value; }
        }
        #endregion

        #region Unity Methods
        [UsedImplicitly]
        private void Awake()
        {
            InitDefaults();
        }

        [UsedImplicitly]
        private void Update()
        {
            UpdateGraph();
        }

        [UsedImplicitly]
        private void OnDestroy()
        {
            Dispose();
        }
        #endregion

        #region Class Methods
        public void InitGraph(RuntimeAnimatorController animatorController, List<Transform> animationTransforms)
        {
            using (new ProfilerScope("Init()"))
            {
                runtimeController = animatorController;

                // Creates the graph, the mixer and binds them to the Animator.
                Animator animator = GetComponentInChildren<Animator>();
                Assert.IsNotNull(animator, $"Prefab '{name}' does not have an {typeof(Animator).Name}, please check if the prefab has been properly setup.");

                mainGraph = PlayableGraph.Create($"Graph_{name}");
                output = AnimationPlayableOutput.Create(mainGraph, "Output", animator);
                controller = AnimatorControllerPlayable.Create(mainGraph, animatorController);

                RebuildFullGraph();

                //todo: GraphVisualizerClient.Show(mainGraph);

                if (animationTransforms != null && animationTransforms.Count > 0)
                {
                    poseHandler.BindStreamTransforms(animator, transform, animationTransforms);
                }

                enabled = fullBodyClips.Count > 0 || additiveClips.Count > 0;
            }
        }

        private void InitDefaults()
        {
            foreach (AnimationClip clip in defaultFullBodyClips)
            {
                AddFullBodyClip(clip);
            }

            AddAdditiveClip(defaultAdditiveClips);

            enabled = false;
        }

        private void Dispose()
        {
            nextMixerState = MixerState.Nothing;

            // Destroys all Playables and Outputs created by the graph.
            if (mainGraph.IsValid())
            {
                mainGraph.Destroy();
            }

            poseHandler.Dispose();
        }

        private void UpdateGraph()
        {
            if (needGraphRebuild)
            {
                needGraphRebuild = false;
                RebuildFullGraph();

                enabled = fullBodyActiveIndex > Consts.INDEX_NONE;
            }

            //Blending is handled here, behaviour automatically disables itself after the blend is fully done (animation played and blend out finished)
            if (fullBodyActiveIndex > Consts.INDEX_NONE)
            {
                if (fullBodyBlendTime > fullBodyClipDuration)
                {
                    AnimationClipPlayable playable = fullBodyPlayables[fullBodyActiveIndex];
                    //playable.Pause();

                    fullBodyActiveIndex = -1;
                    fullBodyClipDuration = 0f;
                    SetupFullBody();

                    enabled = false;
                }
                else
                {
                    fullBodyBlendTime += Time.deltaTime;
                    RefreshFullBodyWeight();
                }
            }
            else
            {
                enabled = false;
            }
        }

        private void RefreshMixerState()
        {
            nextMixerState = MixerState.Nothing;
            nextMixerState |= fullBodyClips.Count > 0 ? MixerState.HasFullBody : MixerState.Nothing;
            nextMixerState |= additiveClips.Count > 0 ? MixerState.HasAdditive : MixerState.Nothing;
            nextMixerState |= nextMixerState.HasEither(MixerState.HasFullBody | MixerState.HasAdditive)
                                ? MixerState.HasPoseHandler : MixerState.Nothing;
        }

        private void RebuildFullGraph()
        {
            RefreshMixerState();
            CleanUp();

            RebuildFullBodyMixer();
            RebuildAdditiveMixer();
            RebuildPoseJob();

            mixerState = nextMixerState;

            RebuildGraph();
        }

        private void CleanUp()
        {
            if (mixerState.HasFlag(MixerState.HasFullBody))
            {
                mainGraph.DisconnectAll(fullBodyMixer);
                mainGraph.DisconnectAll(fullBodySelector);
                fullBodyMixer.Destroy();
                fullBodySelector.Destroy();
            }

            if (mixerState.HasFlag(MixerState.HasAdditive))
            {
                mainGraph.DisconnectAll(additiveMixer);
                additiveMixer.Destroy();
            }

            if (poseHandler.IsPlayableValid && mixerState.HasEither(MixerState.HasPoseHandler))
            {
                mainGraph.DisconnectAll(poseHandler.playableScript);
                poseHandler.playableScript.Destroy();
            }
        }

        private void RebuildFullBodyMixer()
        {
            if (nextMixerState.HasFlag(MixerState.HasFullBody))
            {
                fullBodySelector = AnimationMixerPlayable.Create(mainGraph, fullBodyClips.Count);
                for (int l = 0; l < fullBodyClips.Count; l++)
                {
                    int clipIndex = l;

                    AnimationClip clip = fullBodyClips[clipIndex];
                    AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(mainGraph, clip);
                    mainGraph.Connect(clipPlayable, FIRST_OUTPUT, fullBodySelector, clipIndex);

                    fullBodyPlayables.Add(clipPlayable);
                }

                fullBodyMixer = AnimationMixerPlayable.Create(mainGraph, TWO_INPUT);
                mainGraph.Connect(fullBodySelector, FIRST_OUTPUT, fullBodyMixer, SECOND_INPUT);

                RefreshFullBodyWeight();
            }
        }

        private void RebuildPoseJob()
        {
            if (nextMixerState.HasEither(MixerState.HasPoseHandler))
            {
                poseHandler.Create(mainGraph);
            }
        }

        private void RebuildAdditiveMixer()
        {
            if (nextMixerState.HasFlag(MixerState.HasAdditive))
            {
                additiveMixer = AnimationLayerMixerPlayable.Create(mainGraph, additiveClips.Count + 1);
                for (int c = 0; c < additiveClips.Count; c++)
                {
                    int layerIndex = c + 1;
                    int clipIndex = c;

                    additiveMixer.SetLayerAdditive((uint) layerIndex, true);

                    AnimationClip clip = additiveClips[clipIndex];
                    AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(mainGraph, clip);
                    mainGraph.Connect(clipPlayable, FIRST_OUTPUT, additiveMixer, layerIndex);

                    additivePlayables.Add(clipPlayable);
                    additiveMixer.SetInputWeight(clipIndex, FULL_WEIGHT);
                }
            }
        }

        private void RebuildGraph()
        {
            mainGraph.Stop();

            //Because of typing being too strong, it is impossible to write the node connections in another way ....
            if (mixerState.HasBoth(MixerState.HasFullBody | MixerState.HasAdditive))
            {
                mainGraph.Connect(additiveMixer, FIRST_OUTPUT, fullBodyMixer, FIRST_INPUT);

                if (mixerState.HasFlag(MixerState.HasPoseHandler))
                {
                    mainGraph.Connect(poseHandler.playableScript, FIRST_OUTPUT, additiveMixer, FIRST_INPUT);
                    mainGraph.Connect(controller, FIRST_OUTPUT, poseHandler.playableScript, FIRST_INPUT);
                }
                else
                {
                    mainGraph.Connect(controller, FIRST_OUTPUT, additiveMixer, FIRST_INPUT);
                }
                
                output.SetSourcePlayable(fullBodyMixer);
            }
            else if (mixerState.HasBoth(MixerState.HasFullBody))
            {
                if (mixerState.HasFlag(MixerState.HasPoseHandler))
                {
                    mainGraph.Connect(poseHandler.playableScript, FIRST_OUTPUT, fullBodyMixer, FIRST_INPUT);
                    mainGraph.Connect(controller, FIRST_OUTPUT, poseHandler.playableScript, FIRST_INPUT);
                }
                else
                {
                    mainGraph.Connect(controller, FIRST_OUTPUT, fullBodyMixer, FIRST_INPUT);
                }

                output.SetSourcePlayable(fullBodyMixer);
            }
            else if (mixerState.HasBoth(MixerState.HasAdditive))
            {
                if (mixerState.HasFlag(MixerState.HasPoseHandler))
                {
                    mainGraph.Connect(poseHandler.playableScript, FIRST_OUTPUT, additiveMixer, FIRST_INPUT);
                    mainGraph.Connect(controller, FIRST_OUTPUT, poseHandler.playableScript, FIRST_INPUT);
                }
                else
                {
                    mainGraph.Connect(controller, FIRST_OUTPUT, additiveMixer, FIRST_INPUT);
                }

                output.SetSourcePlayable(additiveMixer);
            }
            else
            {
                output.SetSourcePlayable(controller);
            }

            mainGraph.Evaluate(1f / 30f);
            mainGraph.Play();
        }

        /// <summary>
        /// Add a full-body clip to the graph
        /// </summary>
        /// <param name="clip">The full-body clip</param>
        /// <returns>The index of the added clip, to be used when using Play()</returns>
        public int AddFullBodyClip(AnimationClip clip)
        {
            fullBodyClips.Add(clip);

            enabled = true;
            needGraphRebuild = true;

            return fullBodyClips.Count - 1;
        }

        /// <summary>
        /// Add several additive clips to the graph
        /// </summary>
        /// <param name="clips">The additive clips</param>
        /// <returns>The index of the first added clip, to be used when using Play()</returns>
        public int AddAdditiveClip(List<AnimationClip> clips)
        {
            additiveClips.AddRange(clips);

            enabled = true;
            needGraphRebuild = true;

            return additiveClips.Count - clips.Count;
        }

        /// <summary>
        /// Add an additive clip to the graph
        /// </summary>
        /// <param name="clips">The additive clip</param>
        /// <returns>The index of the added clip, to be used when using Play()</returns>
        public int AddAdditiveClip(AnimationClip clip)
        {
            additiveClips.Add(clip);

            enabled = true;
            needGraphRebuild = true;

            return additiveClips.Count - 1;
        }

        /// <summary>
        /// Play a specific full-body clip, previously registered in the graph
        /// </summary>
        /// <param name="index">The index of the clip</param>
        /// <param name="blendTime">the blend time of the clip</param>
        public void PlayFullBody(int index, float blendTime = 0.1f)
        {
            fullBodyBlendTime = 0;
            fullBodyActiveIndex = index;

            SetupFullBody();

            AnimationClip clip = fullBodyClips[fullBodyActiveIndex];
            fullBodyClipDuration = clip.length;
            this.fullBodyBlendDuration = blendTime;
            this.fullBodyBlendDuration = Mathf.Min(this.fullBodyBlendDuration, fullBodyClipDuration / 2f);

            AnimationClipPlayable playable = fullBodyPlayables[fullBodyActiveIndex];
            //Voodoo magic fix:
            //Setting time once triggers the events in the animation because it considers that it goes from time T -> 0 ....
            //So doing it a second time overwrite the last time to that it does T == 0 in T -> 0 ....
            playable.SetTime(0);
            playable.SetTime(0);
            playable.Play();

            enabled = true;
        }

        private void SetupFullBody()
        {
            int inputs = fullBodySelector.GetInputCount();
            for (int i = 0; i < inputs; i++)
            {
                fullBodySelector.SetInputWeight(i, i == fullBodyActiveIndex ? FULL_WEIGHT : ZERO_WEIGHT);
            }

            RefreshFullBodyWeight();
        }

        private void RefreshFullBodyWeight()
        {
            float alpha = Mathf.Approximately(0, fullBodyBlendDuration) ? 0 : Mathf.Min(Mathf.Clamp01(fullBodyBlendTime / fullBodyBlendDuration), Mathf.Clamp01(fullBodyClipDuration - fullBodyBlendTime) / fullBodyBlendDuration);
            fullBodyMixer.SetInputWeight(FIRST_INPUT, 1f - alpha);
            fullBodyMixer.SetInputWeight(SECOND_INPUT, alpha);
        }
        #endregion
    }
}
