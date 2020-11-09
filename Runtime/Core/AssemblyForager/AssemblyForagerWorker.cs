namespace Prateek.Runtime.Core.AssemblyForager
{
    using System;
    using System.Collections.Generic;

    public abstract class AssemblyForagerWorker
    {
        #region Fields
        private List<(Type, SearchFlag, SearchFlag)> searchInfos = new List<(Type, SearchFlag, SearchFlag)>();
        private List<Type> foundTypes = new List<Type>();
        #endregion

        #region Properties
        public List<Type> FoundTypes { get { return foundTypes; } }
        #endregion

        #region Class Methods
        protected void Search<T>(SearchFlag exclude = SearchFlag.Nothing, SearchFlag include = SearchFlag.Class | SearchFlag.Interface)
        {
            searchInfos.Add((typeof(T), exclude, include));
        }

        protected void Search(params Type[] types)
        {
            foreach (var type in types)
            {
                searchInfos.Add((type, SearchFlag.Nothing, SearchFlag.Class | SearchFlag.Interface));
            }
        }

        protected void Search(params (Type, SearchFlag)[] types)
        {
            foreach (var tuple in types)
            {
                searchInfos.Add((tuple.Item1, tuple.Item2, SearchFlag.Class | SearchFlag.Interface));
            }
        }

        protected void Search(params (Type, SearchFlag, SearchFlag)[] types)
        {
            searchInfos.AddRange(types);
        }

        internal void TryStore(Type assemblyType)
        {
            foreach (var tuple in searchInfos)
            {
                var searchedType = tuple.Item1;
                var exclude      = tuple.Item2;
                var include      = tuple.Item3;
                if (exclude != SearchFlag.Nothing && (GetFlags(assemblyType) & exclude) != SearchFlag.Nothing
                 || include != SearchFlag.Nothing && (GetFlags(assemblyType) & include) == SearchFlag.Nothing)
                {
                    continue;
                }

                if (assemblyType == searchedType
                 || assemblyType.IsSubclassOf(searchedType)
                 || searchedType.IsAssignableFrom(assemblyType))
                {
                    foundTypes.Add(assemblyType);
                    break;
                }
            }
        }

        private SearchFlag GetFlags(Type assemblyType)
        {
            var flag = SearchFlag.Nothing;
            flag |= assemblyType.IsClass ? SearchFlag.Class : flag;
            flag |= assemblyType.IsInterface ? SearchFlag.Interface : flag;
            flag |= assemblyType.IsValueType ? SearchFlag.Struct : flag;
            flag |= assemblyType.IsEnum ? SearchFlag.Enum : flag;
            flag |= assemblyType.IsAbstract ? SearchFlag.Abstract : flag;
            flag |= assemblyType.IsSealed ? SearchFlag.Sealed : flag;
            return flag;
        }

        internal virtual void Init()
        {
            PrepareSearch();
        }

        public abstract void PrepareSearch();
        public virtual void WorkDone() { }
        #endregion

        #region SearchFlag enum
        [Flags]
        protected enum SearchFlag
        {
            Nothing = 0,
            Class = 1 << 0,
            Interface = 1 << 1,
            Struct = 1 << 2,
            Enum = 1 << 3,
            Abstract = 1 << 4,
            Sealed = 1 << 5,
        }
        #endregion
    }
}
