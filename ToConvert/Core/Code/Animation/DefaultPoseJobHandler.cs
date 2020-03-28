namespace Mayfair.Core.Code.Animation
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.Types.Extensions;
    using Unity.Collections;
    using UnityEngine;
    using UnityEngine.Animations;
    using UnityEngine.Playables;

    public class DefaultPoseJobHandler : IDisposable
    {
        #region Fields
        public UnityEngine.Animations.AnimationScriptPlayable playableScript;
        private UnityEngine.Animations.TransformStreamHandle rootHandle;
        private NativeArray<UnityEngine.Animations.TransformStreamHandle> boneHandles;
        private NativeArray<Vector3> localPositions;
        private NativeArray<Quaternion> localRotations;
        private NativeArray<Vector3> localScales;
        #endregion

        #region Properties
        public bool IsPlayableValid
        {
            get { return playableScript.IsValid(); }
        }
        #endregion

        #region Class Methods
        public void BindStreamTransforms(Animator animator, Transform root, List<Transform> sourceTransforms)
        {
            //Setting default
            rootHandle = animator.BindStreamTransform(root);
            boneHandles = new NativeArray<UnityEngine.Animations.TransformStreamHandle>(sourceTransforms.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            localPositions = new NativeArray<Vector3>(sourceTransforms.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            localRotations = new NativeArray<Quaternion>(sourceTransforms.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            localScales = new NativeArray<Vector3>(sourceTransforms.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

            for (int i = 0; i < sourceTransforms.Count; ++i)
            {
                //This is needed because the multithreaded nature of the animation job system makes it mandatory to use wrapper classes to manage transforms
                boneHandles[i] = animator.BindStreamTransform(sourceTransforms[i]);

                localPositions[i] = sourceTransforms[i].localPosition;
                localRotations[i] = sourceTransforms[i].localRotation;
                localScales[i] = sourceTransforms[i].localScale;
            }
        }

        public void Create(PlayableGraph graph)
        {
            DefaultPoseJob dampingJob = new DefaultPoseJob
            {
                rootHandle = rootHandle,
                jointHandles = boneHandles,
                localPositions = localPositions,
                localRotations = localRotations,
                localScales = localScales
            };

            playableScript = UnityEngine.Animations.AnimationScriptPlayable.Create(graph, dampingJob, 1);
            playableScript.SetInputWeight(0, 1);
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            if (playableScript.IsValid())
            {
                playableScript.Destroy();
            }

            boneHandles.SafeDispose();
            localPositions.SafeDispose();
            localRotations.SafeDispose();
            localScales.SafeDispose();
        }
        #endregion
    }
}
