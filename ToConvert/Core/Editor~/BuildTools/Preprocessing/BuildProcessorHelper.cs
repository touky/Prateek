namespace Mayfair.Core.Editor.BuildTools.Preprocessing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Interfaces;
    using UnityEditor;
    using UnityEditor.Build.Reporting;
    using UnityEngine;

    public class BuildProcessorHelper
    {
        public static void RunPreprocessMethods(BuildTarget buildTarget)
        {
            Debug.Log("[BuildProcessorHelper] Beginning custom build preprocessing");
            List<IPreprocessBuild> buildPreprocessors = CollectFromAssemblies<IPreprocessBuild>();
            buildPreprocessors.Sort(OrderedCallbackSort);
            Debug.Log($"[BuildProcessorHelper] {buildPreprocessors.Count} preprocessors found");
            PreprocessBuild(buildPreprocessors, buildTarget);
        }

        public static void RunPostprocessMethods(BuildSummary buildSummary)
        {
            Debug.Log("[BuildProcessorHelper] Beginning custom build postprocessing");
            List<IPostprocessBuild> buildPostprocessors = CollectFromAssemblies<IPostprocessBuild>();
            buildPostprocessors.Sort(OrderedCallbackSort);
            Debug.Log($"[BuildProcessorHelper] {buildPostprocessors.Count} postprocessors found");
            PostprocessBuild(buildPostprocessors, buildSummary);
        }

        private static List<T> CollectFromAssemblies<T>() where T : class
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type classType = typeof(T);
            List<T> classReferences = new List<T>();
            foreach (Assembly assembly in assemblies)
            {
                IEnumerable<Type> classTypes = assembly.GetTypes().Where(type => type.IsClass &&
                                                                                    !type.IsAbstract &&
                                                                                    classType.IsAssignableFrom(type));

                foreach (Type type in classTypes)
                {
                    classReferences.Add(Activator.CreateInstance(type) as T);
                }
            }

            return classReferences;
        }

        private static int OrderedCallbackSort(IOrderedCallback a, IOrderedCallback b)
        {
            if (a == null && b == null)
            {
                return 0;
            }

            if (a == null)
            {
                return -1;
            }

            if (b == null)
            {
                return 1;
            }

            return a.CallbackOrder.CompareTo(b.CallbackOrder);
        }

        private static void PreprocessBuild(List<IPreprocessBuild> buildPreprocessors, BuildTarget buildTarget)
        {
            foreach (IPreprocessBuild buildPreprocessor in buildPreprocessors)
            {
                Debug.Log($"[BuildProcessorHelper] Running preprocessor {buildPreprocessor.GetType().Name}");
                buildPreprocessor.OnPreprocessBuild(buildTarget);
            }
        }

        private static void PostprocessBuild(List<IPostprocessBuild> buildPostprocessors, BuildSummary buildSummary)
        {
            foreach (IPostprocessBuild buildPostprocessor in buildPostprocessors)
            {
                Debug.Log($"[BuildProcessorHelper] Running postprocessor {buildPostprocessor.GetType().Name}");
                buildPostprocessor.OnPostprocessBuild(buildSummary);
            }
        }
    }
}