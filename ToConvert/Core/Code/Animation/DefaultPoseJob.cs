namespace Mayfair.Core.Code.Animation
{
    using Unity.Collections;
    using UnityEngine;
    

    public struct DefaultPoseJob : UnityEngine.Animations.IAnimationJob
    {
        public UnityEngine.Animations.TransformStreamHandle rootHandle;
        public NativeArray<UnityEngine.Animations.TransformStreamHandle> jointHandles;
        public NativeArray<Vector3> localPositions;
        public NativeArray<Quaternion> localRotations;
        public NativeArray<Vector3> localScales;

        /// <summary>
        ///     Process the root motion content in the animation
        /// </summary>
        /// <param name="stream">The animation stream</param>
        public void ProcessRootMotion(UnityEngine.Animations.AnimationStream stream)
        {
            // Always zero-out the root, this job doesn't support root motion
            rootHandle.SetLocalPosition(stream, Vector3.zero);
            rootHandle.SetLocalRotation(stream, Quaternion.identity);
        }

        /// <summary>
        ///     Process each joint in the stream
        ///     Set their local transform base on the given values
        /// </summary>
        /// <param name="stream">The animation stream</param>
        public void ProcessAnimation(UnityEngine.Animations.AnimationStream stream)
        {
            if (jointHandles.Length < 2)
            {
                return;
            }

            for (int i = 0; i < jointHandles.Length; ++i)
            {
                UnityEngine.Animations.TransformStreamHandle jointHandle = jointHandles[i];

                jointHandle.SetLocalPosition(stream, localPositions[i]);
                jointHandle.SetLocalRotation(stream, localRotations[i]);
                jointHandle.SetLocalScale(stream, localScales[i]);
            }
        }
    }
}
