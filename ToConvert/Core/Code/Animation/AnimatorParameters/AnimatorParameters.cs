namespace Mayfair.Core.Code.Animation.AnimatorParameters
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;
    using Mayfair.Core.Code.Utils.Debug.Reflection;
    using UnityEngine;
    using UnityEngine.Animations;
    using UnityEngine.Assertions;

    [Serializable]
    public class AnimatorParameters
    {
        #region Class Methods
        public void Validate(Animator animator)
        {
            Validate(new List<AnimatorControllerParameter>(animator.parameters), animator.name);
        }

        public void Validate(AnimatorControllerPlayable controller)
        {
            List<AnimatorControllerParameter> list = new List<AnimatorControllerParameter>();
            int count = controller.GetParameterCount();
            for (int c = 0; c < count; c++)
            {
                list.Add(controller.GetParameter(c));
            }

            Validate(list, controller.ToString());
        }

        [Conditional("NVIZZIO_DEV")]
        private void Validate(List<AnimatorControllerParameter> parameters, string name)
        {
#if UNITY_EDITOR
            StringBuilder builder = null;
            List<AnimatorControllerParameter> existingParameters = new List<AnimatorControllerParameter>(parameters);
            FieldInfo[] fieldInfos = ReflectionUtils.GetMembers(GetType(), true);
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                ReflectedField<AnimatorProperty> property = fieldInfo.Name;
                property.Init(this);

                AnimatorControllerParameter foundParam = existingParameters.Find(x => { return x.name == property.Value.Name; });
                if (!AssertIsTrue(ref builder, foundParam != null, $"{fieldInfo.Name} with value '{property.Value.Name}' not found in '{name}'"))
                {
                    continue;
                }

                Type paramType = fieldInfo.FieldType;
                if (paramType == typeof(AnimatorProperty))
                {
                    continue;
                }

                bool typeIsWrong = false;
                if (foundParam.type == AnimatorControllerParameterType.Trigger)
                {
                    typeIsWrong = paramType != typeof(AnimatorPropertyTrigger);
                }
                else
                {
                    typeIsWrong = ValidateProperty(paramType, foundParam);
                }

                AssertIsTrue(ref builder, !typeIsWrong, $"{fieldInfo.Name} with value '{property.Value.Name}' is not of type '{foundParam.type}'");
            }

            if (builder != null)
            {
                Assert.IsTrue(false, builder.ToString());
            }
#endif
        }

        private bool ValidateProperty(Type paramType, AnimatorControllerParameter foundParam)
        {
#if UNITY_EDITOR
            while (paramType != typeof(AnimatorProperty))
            {
                TypeInfo typeInfo = paramType.GetTypeInfo();
                if (typeInfo.GenericTypeArguments.Length > 0)
                {
                    paramType = typeInfo.GenericTypeArguments[0];

                    switch (foundParam.type)
                    {
                        case AnimatorControllerParameterType.Float:
                        {
                            return paramType != typeof(float);
                        }
                        case AnimatorControllerParameterType.Int:
                        {
                            return paramType != typeof(int);
                        }
                        case AnimatorControllerParameterType.Bool:
                        {
                            return paramType != typeof(bool);
                        }
                        default:
                        {
                            return false;
                        }
                    }
                }

                paramType = paramType.BaseType;
            }
#endif
            return false;
        }

#if UNITY_EDITOR
        private bool AssertIsTrue(ref StringBuilder builder, bool isTrue, string line)
        {
            if (isTrue)
            {
                return true;
            }

            if (builder == null)
            {
                builder = new StringBuilder();
            }

            builder.AppendLine(line);

            return false;
        }
#endif
        #endregion
    }
}
