namespace Prateek.Runtime.Core.AssemblyForager
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    internal static class AssemblyForager
    {
        #region Static and Constants
        private const int TYPE_COUNT = 30000;
        internal static List<AssemblyForagerWorker> workers = new List<AssemblyForagerWorker>();
        #endregion

        #region Class Methods
        //todo: editor support [InitializeOnLoadMethod]
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Execute()
        {
            var builder = (StringBuilder) null;
            var types   = new List<Type>(TYPE_COUNT);
            foreach (var domainAssembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var assemblyTypes = domainAssembly.GetTypes();
                foreach (var assemblyType in assemblyTypes)
                {
                    types.Add(assemblyType);

                    if (!assemblyType.IsSubclassOf(typeof(AssemblyForagerWorker)))
                    {
                        continue;
                    }

                    if (assemblyType.IsAbstract || assemblyType.IsInterface)
                    {
                        continue;
                    }

                    var worker = Activator.CreateInstance(assemblyType) as AssemblyForagerWorker;
                    worker.Init();
                    workers.Add(worker);
                    LogWorker(ref builder, worker);
                }
            }

            builder.Log("- Starting type lookup");
            foreach (var assemblyType in types)
            {
                foreach (var worker in workers)
                {
                    worker.TryStore(assemblyType);
                }
            }

            builder.Log("- Calling WorkDone");
            foreach (var worker in workers)
            {
                worker.WorkDone();
            }

            builder.Log("- Clearing workers");
            workers.Clear();
        }

        private static void LogWorker(ref StringBuilder builder, AssemblyForagerWorker worker)
        {
            if (builder == null)
            {
                builder = new StringBuilder();
                builder.AppendLine($"{nameof(AssemblyForager)} report:");
            }

            builder.AppendLine($"- Found worker: {worker.GetType().Name}");
        }

        private static void Log(this StringBuilder builder, string log)
        {
            if (builder == null)
            {
                return;
            }

            builder.AppendLine(log);
        }
        #endregion
    }
}
