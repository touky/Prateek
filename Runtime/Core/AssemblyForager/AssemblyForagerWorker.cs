using System;
using System.Collections.Generic;

namespace Prateek.Runtime.Core.AssemblyForager
{
    public abstract class AssemblyForagerWorker
    {
        #region Fields
        private List<Type> searchedTypes = new List<Type>();
        private List<Type> foundTypes = new List<Type>();
        #endregion

        #region Properties
        public List<Type> FoundTypes
        {
            get { return foundTypes; }
        }
        #endregion

        #region Class Methods
        protected void Search<T>()
        {
            searchedTypes.Add(typeof(T));
        }

        protected void Search(params Type[] types)
        {
            searchedTypes.AddRange(types);
        }

        internal void TryStore(Type assemblyType)
        {
            foreach (var searchedType in searchedTypes)
            {
                if (assemblyType == searchedType
                 || assemblyType.IsSubclassOf(searchedType)
                 || searchedType.IsAssignableFrom(assemblyType))
                {
                    foundTypes.Add(assemblyType);
                    break;
                }
            }
        }

        public virtual void Init()
        {
            throw new NotImplementedException();
        }

        public virtual void WorkDone() { }
        #endregion
    }
}
