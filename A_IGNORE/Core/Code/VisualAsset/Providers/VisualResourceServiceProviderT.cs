namespace Mayfair.Core.Code.VisualAsset.Providers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using JetBrains.Annotations;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Debug;
    using Mayfair.Core.Code.Utils.Extensions;
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader.Interfaces;
    using Prateek.Runtime.KeynameFramework;
    using Prateek.Runtime.KeynameFramework.Interfaces;
    using UnityEngine.Assertions;

    public abstract class VisualResourceServant<TResourceReference>
        : VisualResourceServant
        where TResourceReference : class, IContentHandle
    {
        #region Fields
        protected Dictionary<string, Dictionary<Keyname, IContentHandle>> resources = new Dictionary<string, Dictionary<Keyname, IContentHandle>>();
        private List<SupportedAssignable> supportedAssignables = new List<SupportedAssignable>();
        #endregion

        #region Unity Methods
        public override void RefreshPending()
        {
            foreach (SupportedAssignable container in supportedAssignables)
            {
                TryRefreshingPendingInstances(container);
            }
        }
        #endregion

        #region Class Methods
        protected void AddSupportedAssignable(string keyword, string defaultResource)
        {
            supportedAssignables.Add(new SupportedAssignable(keyword, defaultResource));
        }

        protected void AddPendingInit(int assignableIndex, IAssignableVisualResource resource)
        {
            supportedAssignables[assignableIndex].pendingInit.AddUnique(resource);
        }

        private void TryRefreshingPendingInstances(SupportedAssignable container)
        {
            if (container.pendingInit.Count == 0)
            {
                return;
            }

            Dictionary<Keyname, IContentHandle> references = GetReferences(container.Keyword);
            if (references == null)
            {
                //Not yet loaded, wait for that
                return;
            }

            for (int i = 0; i < container.pendingInit.Count; i++)
            {
                IAssignableVisualResource assignable = container.pendingInit[i];
                IKeynameUser keynameUser = assignable as IKeynameUser;
                if (keynameUser == null)
                {
                    throw new Exception($"MAJOR ERROR ! {assignable} is not an IIdentifiable");
                }

                IContentHandle iRreference;
                if (!references.TryGetValue(keynameUser.Keyname, out iRreference))
                {
                    string logText = "//TODO: FEEDBACK"; //todo: $"{GetType().Name}: Visual resource for {keynameUser.Keyname.RawValue} could not be found, FIX IT !";
                    if (!references.TryGetValue(container.DefaultResource, out iRreference))
                    {
                        //Upgrade to error
                        DebugTools.LogError(logText);

                        //Remove from pending, this won't work
                        container.pendingInit.RemoveAt(i--);
                        continue;
                    }

                    //19/12/12 Return a default reference for undefinied buildings
                    DebugTools.LogWarning(logText);
                }

                TResourceReference reference = iRreference as TResourceReference;
                Assert.IsNotNull(reference);

                //Remove from init here, because it either is good, or to be handled in LoadCompleted()
                container.pendingInit.RemoveAt(i--);

                if (!reference.Loader.IsDone)
                {
                    reference.LoadCompleted = ResourceLoadCompleted;
                    reference.LoadAsync();

                    AddToLoad(container, reference, assignable);
                    continue;
                }

                assignable.Assign(reference);
            }
        }

        private void AddToLoad(SupportedAssignable container, TResourceReference reference, IAssignableVisualResource instance)
        {
            HashSet<IAssignableVisualResource> instances = null;
            if (!container.pendingLoad.TryGetValue(reference, out instances))
            {
                instances = new HashSet<IAssignableVisualResource>();
                container.pendingLoad.Add(reference, instances);
            }

            instances.Add(instance);
        }

        private void ResourceLoadCompleted(IContentHandle reference)
        {
            foreach (SupportedAssignable container in supportedAssignables)
            {
                if (!container.pendingLoad.ContainsKey(reference))
                {
                    continue;
                }

                HashSet<IAssignableVisualResource> assignables = null;
                if (!container.pendingLoad.TryGetValue(reference, out assignables))
                {
                    throw new Exception($"reference {reference.Loader.Path} couldn't be found, this is a fail-state");
                }

                foreach (IAssignableVisualResource assignable in assignables)
                {
                    assignable.Assign(reference);
                }

                container.pendingLoad.Remove(reference);

                return;
            }
        }

        protected void Store(TResourceReference resource)
        {
            string location = resource.Loader.Path;
            int index = location.IndexOf(Path.AltDirectorySeparatorChar);
            string root = location.Substring(0, index);
            string name = location.Substring(index + Consts.NEXT_ITEM, location.Length - (index + Consts.NEXT_ITEM));

            Dictionary<Keyname, IContentHandle> references = null;
            if (!resources.TryGetValue(root, out references))
            {
                references = new Dictionary<Keyname, IContentHandle>();
                resources.Add(root, references);
            }

            Keyname keyname = Keyname.Create(name);
            if (!references.ContainsKey(keyname))
            {
                references.Add(keyname, resource);
            }
            else
            {
                references[keyname] = resource;
            }
        }

        protected Dictionary<Keyname, IContentHandle> GetReferences(string key)
        {
            Dictionary<Keyname, IContentHandle> result;
            if (!resources.TryGetValue(key, out result))
            {
                return null;
            }

            return result;
        }
        #endregion

        #region Nested type: SupportedAssignable
        private class SupportedAssignable
        {
            #region Fields
            private string keyword = null;
            private Keyname defaultResource;
            public List<IAssignableVisualResource> pendingInit = new List<IAssignableVisualResource>();
            public Dictionary<IContentHandle, HashSet<IAssignableVisualResource>> pendingLoad = new Dictionary<IContentHandle, HashSet<IAssignableVisualResource>>();
            #endregion

            #region Properties
            public string Keyword
            {
                get { return keyword; }
            }

            public string DefaultResource
            {
                get { return defaultResource.ToString(); }
            }

            public virtual Type Type
            {
                get { return null; }
            }
            #endregion

            #region Constructors
            public SupportedAssignable(string keyword, string defaultResource)
            {
                this.keyword = keyword;
                this.defaultResource = defaultResource;
            }
            #endregion
        }
        #endregion
    }
}
