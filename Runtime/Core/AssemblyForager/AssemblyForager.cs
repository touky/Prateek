namespace Prateek.Runtime.Core.AssemblyForager
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Prateek.Runtime.Core.Consts;
    using UnityEditor;
    using UnityEngine;

    internal static class AssemblyForager
    {
        #region Static and Constants
        private const int TYPE_COUNT = 30000;

        private static readonly string FORAGE_INSTRUCTION = $"{ConstFolder.PRATEEK_SETTINGS}/{nameof(AssemblyForager)}{nameof(LookupInstructions)}{ConstExtension.JSON}";

        internal static List<AssemblyForagerWorker> workers = new List<AssemblyForagerWorker>();
        #endregion

        #region Class Methods
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void EditorForage()
        {
            Forage();
        }
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RuntimeForage()
        {
            Forage();
        }

        private static void Forage()
        {
            var instructions = LookupInstructions.Load(FORAGE_INSTRUCTION, () => new PrateekDefaultInstructions());

            Execute(instructions);

#if UNITY_EDITOR
            instructions.Save(FORAGE_INSTRUCTION);
#endif
        }

        private static void Execute(LookupInstructions instructions)
        {
            var builder    = (StringBuilder) null;
            var types      = new List<Type>(TYPE_COUNT);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var domainAssembly in assemblies)
            {
                var assemblyTypes = (Type[]) null;
                var shortName = domainAssembly.FullName.Substring(0, domainAssembly.FullName.IndexOf(Const.COMMA_C));
                var allowWorkerLookup = instructions.Allow(shortName);
                var allowTypeLookup = instructions.Allow(shortName, true);
                if (!allowWorkerLookup)
                {
                    builder.Log($"Ignoring {domainAssembly.FullName}");
                    if (allowTypeLookup)
                    {
                        assemblyTypes = domainAssembly.GetTypes();
                        foreach (var assemblyType in assemblyTypes)
                        {
                            if (!assemblyType.IsSubclassOf(typeof(AssemblyForagerWorker)))
                            {
                                types.Add(assemblyType);
                                continue;
                            }
                        }
                    }
                    continue;
                }

                assemblyTypes = domainAssembly.GetTypes();
                foreach (var assemblyType in assemblyTypes)
                {
                    if (!assemblyType.IsSubclassOf(typeof(AssemblyForagerWorker)))
                    {
                        types.Add(assemblyType);
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
