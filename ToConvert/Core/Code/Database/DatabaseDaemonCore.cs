namespace Mayfair.Core.Code.Database
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Mayfair.Core.Code.Database.Interfaces;
    using Mayfair.Core.Code.Database.Messages;
    using Mayfair.Core.Code.Database.Messages.RequestFilters;
    using Mayfair.Core.Code.Database.ServiceProvider;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.Resources;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Debug;
    using Mayfair.Core.Code.Utils.Types;
    using Mayfair.Core.Code.Utils.Types.UniqueId;
    using Prateek.NoticeFramework.Notices.Core;
    using DatabaseContentByFilterRequest = Mayfair.Core.Code.Database.Messages.DatabaseContentMatchingWithFilterRequest<Messages.DatabaseContentMatchingWithFilterResponse>;

    public sealed class DatabaseDaemonCore : ResourceDependentDaemonCore<DatabaseDaemonCore, DatabaseDaemonBranch>, IDebugMenuNotebookOwner
    {
        #region IdentifierRequestStatus enum
        private enum IdentifierRequestStatus
        {
            NotInitialized,
            WaitingForIdentifiers,
            Received
        }

        private enum DatabaseRebuildStatus
        {
            Ready,
            IsRebuilding,
            ShouldRebuild,
        }
        #endregion

        #region Fields
        private Dictionary<Type, List<IDatabaseEntry>> databaseEntries = new Dictionary<Type, List<IDatabaseEntry>>();
        private HashSet<CompositeIdentifier> identifiers = new HashSet<CompositeIdentifier>();
        private Queue<CompositeIdentifier> identifiersToRebuild = new Queue<CompositeIdentifier>();

        private readonly Dictionary<Keyname, ICompositeContent> allCompositeContents = new Dictionary<Keyname, ICompositeContent>();

        private readonly Queue<DatabaseContentByIdRequest> requestsForContentHold = new Queue<DatabaseContentByIdRequest>();
        private readonly Queue<DatabaseContentByFilterRequest> requestsWithFilterHold = new Queue<DatabaseContentByFilterRequest>();

        private IdentifierRequestStatus identifierStatus = IdentifierRequestStatus.NotInitialized;
        private ResourcesLoadingStatus resourcesLoadingStatus = ResourcesLoadingStatus.NotStarted;

        private DatabaseRebuildStatus rebuildStatus;
        #endregion

        #region Properties
        protected override ServiceProviderUsageRuleType ServiceProviderUsageRule
        {
            get { return ServiceProviderUsageRuleType.UseFirstValid; }
        }
        #endregion

        #region Unity Methods
        protected override void Start()
        {
            base.Start();

            SetupDebugContent();
        }

        protected override void Update()
        {
            base.Update();

            RefreshPendingResources();

            if (rebuildStatus > DatabaseRebuildStatus.Ready)
            {
                RefreshContentRebuild();
            }
        }
        #endregion

        #region Service
        protected override void OnAwake() { }
        #endregion

        #region Messaging
        public override void NoticeReceived()
        {
            //Empty
        }

        protected override void SetupNoticeReceiverCallback()
        {
            base.SetupNoticeReceiverCallback();

            NoticeReceiver.AddCallback<DatabaseIdentifierResponse>(OnIdentifiersReceived);
            NoticeReceiver.AddCallback<DatabaseContentByIdRequest>(OnRequestContentById);
            NoticeReceiver.AddCallback<DatabaseContentByFilterRequest>(OnDatabaseContentMatchingFilter);
        }
        #endregion

        #region Class Methods
        private void OnIdentifiersReceived(DatabaseIdentifierResponse response)
        {
            foreach (ICompositeIdentifier identifier in response.Identifiers)
            {
                AddIdentifier(identifier);
            }

            identifierStatus = IdentifierRequestStatus.Received;

            if (identifierStatus == IdentifierRequestStatus.Received && resourcesLoadingStatus == ResourcesLoadingStatus.Loaded)
            {
                rebuildStatus = DatabaseRebuildStatus.ShouldRebuild;
            }
        }

        public void SetResourcesLoadingStatus(ResourcesLoadingStatus status)
        {
            resourcesLoadingStatus = status;

            if (identifierStatus == IdentifierRequestStatus.Received && status == ResourcesLoadingStatus.Loaded)
            {
                rebuildStatus = DatabaseRebuildStatus.ShouldRebuild;

                allCompositeContents.Clear();
                identifiersToRebuild.Clear();
                foreach (CompositeIdentifier identifier in identifiers)
                {
                    identifiersToRebuild.Enqueue(identifier);
                }
            }
        }

        private void OnRequestContentById(DatabaseContentByIdRequest request)
        {
            if (!identifierStatus.Equals(IdentifierRequestStatus.Received))
            {
                requestsForContentHold.Enqueue(request);
                SendRequestForIdentifiers();
                return;
            }

            if (!resourcesLoadingStatus.Equals(ResourcesLoadingStatus.Loaded))
            {
                requestsForContentHold.Enqueue(request);
                return;
            }

            DatabaseContentByIdResponse response = request.GetResponse();

            for (int i = 0, n = request.UniqueIds.Count; i < n; i++)
            {
                Keyname keyname = request.UniqueIds[i];

                //Name can only match as equal, so directly try-get
                if (keyname.Type == KeynameState.Fullname)
                {
                    if (allCompositeContents.TryGetValue(keyname, out ICompositeContent compositeContentFound))
                    {
                        response.Content.Add(compositeContentFound);
                    }
                }

                //For a filter, use the IdMatchRequirement to ensure it's correctly matched
                else if (keyname.Type == KeynameState.Keywords)
                {
                    foreach (KeyValuePair<Keyname, ICompositeContent> content in allCompositeContents)
                    {
                        if (keyname.Match(content.Key) <= request.IdMatchRequirement)
                        {
                            response.Content.Add(content.Value);
                        }
                    }
                }
            }

            NoticeReceiver.Send(response);
        }

        private void SendRequestForIdentifiers()
        {
            if (!identifierStatus.Equals(IdentifierRequestStatus.WaitingForIdentifiers))
            {
                identifierStatus = IdentifierRequestStatus.WaitingForIdentifiers;
                DatabaseIdentifierRequest<DatabaseIdentifierResponse> notice =
                    Notice.Create<DatabaseIdentifierRequest<DatabaseIdentifierResponse>>();

                NoticeReceiver.Send(notice);
            }
        }

        private void OnDatabaseContentMatchingFilter(DatabaseContentByFilterRequest request)
        {
            if (!identifierStatus.Equals(IdentifierRequestStatus.Received))
            {
                requestsWithFilterHold.Enqueue(request);
                SendRequestForIdentifiers();
                return;
            }

            if (!resourcesLoadingStatus.Equals(ResourcesLoadingStatus.Loaded))
            {
                requestsWithFilterHold.Enqueue(request);
                return;
            }

            DatabaseContentMatchingWithFilterResponse response = request.GetResponse();

            if (request.Operator == FilterLogicalOperators.AND)
            {
                PatternContainsAll(request.Filters, response.Content);
            }
            else
            {
                PatternContainsAny(request.Filters, response.Content);
            }

            DebugTools.Log(this, "OnDatabaseContentMatchingFilter request received and handled. Sending result now");

            NoticeReceiver.Send(response);
        }

        private void PatternContainsAll(List<string> filters, List<ICompositeContent> results)
        {
            foreach (KeyValuePair<Keyname, ICompositeContent> pair in allCompositeContents)
            {
                bool valid = true;
                string uniqueIdAsString = pair.Key.RawValue;
                for (int i = 0, n = filters.Count; i < n; i++)
                {
                    if (!uniqueIdAsString.Contains(filters[i]))
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    results.Add(pair.Value);
                }
            }
        }

        private void PatternContainsAny(List<string> filters, List<ICompositeContent> results)
        {
            foreach (KeyValuePair<Keyname, ICompositeContent> pair in allCompositeContents)
            {
                bool valid = false;
                string uniqueIdAsString = pair.Key.RawValue;
                for (int i = 0, n = filters.Count; i < n; i++)
                {
                    if (uniqueIdAsString.Contains(filters[i]))
                    {
                        valid = true;
                        break;
                    }
                }

                if (valid)
                {
                    results.Add(pair.Value);
                }
            }
        }

        private void RefreshPendingResources()
        {
            DatabaseDaemonBranch branch = GetFirstAliveBranch();
            if (branch == null)
            {
                return;
            }

            branch.RefreshPendingResources(this);
        }

        public static ICompositeIdentifier CreateNewIdentifier<T>() where T : IDatabaseEntry
        {
            return new CompositeIdentifier(typeof(T));
        }

        public void AddEntry(IDatabaseEntry entry)
        {
            List<IDatabaseEntry> list = null;
            if (list == null)
            {
                if (!databaseEntries.TryGetValue(entry.GetType(), out list))
                {
                    list = new List<IDatabaseEntry>();
                    databaseEntries.Add(entry.GetType(), list);
                }
            }

            list.Add(entry);
        }

        private void AddIdentifier(ICompositeIdentifier identifier)
        {
            CompositeIdentifier composite = identifier as CompositeIdentifier;
            if (composite == null)
            {
                throw new Exception("Invalid Composite type, useGUILayout NewIdentifier<T>()");
            }

            if (!identifiers.Contains(composite))
            {
                identifiers.Add(composite);
                identifiersToRebuild.Enqueue(composite);
            }
        }

        private void RefreshContentRebuild()
        {
            rebuildStatus = DatabaseRebuildStatus.IsRebuilding;

#if NVIZZIO_DEV
            using (new ProfilerScope("RefreshContentRebuild()"))
#endif
            {
                while (true)
                //foreach (CompositeIdentifier identifier in identifiers)
                {
                    CompositeIdentifier identifier = identifiersToRebuild.Dequeue();
#if NVIZZIO_DEV
                    using (new ProfilerScope("foreach(identifiers)"))
#endif
                    {
                        if (!databaseEntries.TryGetValue(identifier.rootType, out List<IDatabaseEntry> dataEntries))
                        {
                            //On fail, continue with the next identifier
                            continue;
                        }

#if NVIZZIO_DEV
                        using (new ProfilerScope($"using {identifier.rootType.Name}"))
#endif
                        {
                            foreach (IDatabaseEntry dataEntry in dataEntries)
                            {
#if NVIZZIO_DEV
                                using (new ProfilerScope("foreach(dataEntries)"))
#endif
                                {
                                    ICompositeContent content = identifier.BuildCompositeContent(dataEntry.IdUnique, databaseEntries);
                                    if (content == null)
                                    {
                                        continue;
                                    }

                                    allCompositeContents.Add(content.Keyname, content);
                                }
                            }
                        }
                    }

                    //Stop after the first iteration
                    break;
                }
            }

            if (identifiersToRebuild.Count == 0)
            {
                DatabaseReadyToBeUsed();
            }
        }

        private void DatabaseReadyToBeUsed()
        {
            DebugTools.Log(this, $"Identifiers status: {identifierStatus} | resources status: {resourcesLoadingStatus} | composite contents count: {allCompositeContents.Count}");

            rebuildStatus = DatabaseRebuildStatus.Ready;

            if (identifierStatus.Equals(IdentifierRequestStatus.Received) && resourcesLoadingStatus.Equals(ResourcesLoadingStatus.Loaded))
            {
                HandlePostponedRequests();
            }
        }

        private void HandlePostponedRequests()
        {
            DebugTools.Log(this, "Handle postponed requests");

            // Evaluate requests that was postponed because we didn't have the identifiers or resources
            for (int i = 0, n = requestsForContentHold.Count; i < n; i++)
            {
                OnRequestContentById(requestsForContentHold.Dequeue());
            }

            for (int i = 0, n = requestsWithFilterHold.Count; i < n; i++)
            {
                OnDatabaseContentMatchingFilter(requestsWithFilterHold.Dequeue());
            }
        }

        #region Debug
        [Conditional("NVIZZIO_DEV")]
        public void SetupDebugContent()
        {
            DebugMenuNotebook debugNotebook = new DebugMenuNotebook("DTBS", "Database Service");

            EmptyMenuPage main = new EmptyMenuPage("MAIN");
            debugNotebook.AddPagesWithParent(main, new DatabaseEntryMenuPage(this, "Database entries"));
            debugNotebook.Register();
        }
        #endregion
        #endregion

        #region Nested type: CompositeContent
        private class CompositeContent : ICompositeContent
        {
            #region Fields
            public CompositeIdentifier identifier;
            public Keyname keyname;
            public List<IDatabaseEntry> contents = new List<IDatabaseEntry>();
            #endregion

            #region Constructors
            public CompositeContent(Keyname keyname, CompositeIdentifier identifier)
            {
                this.keyname = keyname;
                this.identifier = identifier;
            }
            #endregion

            #region Class Methods
            public void AddContent(IDatabaseEntry content)
            {
                contents.Add(content);
            }
            #endregion

            #region ICompositeContent Members
            public Keyname Keyname
            {
                get { return keyname; }
            }

            public bool Contains<TType>() where TType : IDatabaseEntry
            {
                return (TType) Get(typeof(TType)) != null;
            }

            public bool Contains<TSource, TJoiner>() where TSource : IDatabaseEntry
                                                     where TJoiner : IDatabaseEntry
            {
                return (TJoiner) Get(typeof(TSource), typeof(TJoiner)) != null;
            }

            public TType Get<TType>() where TType : IDatabaseEntry
            {
                return (TType) Get(typeof(TType));
            }

            public TJoiner Get<TSource, TJoiner>() where TSource : IDatabaseEntry
                                                   where TJoiner : IDatabaseEntry
            {
                return (TJoiner) Get(typeof(TSource), typeof(TJoiner));
            }

            public IDatabaseEntry Get(Type source)
            {
                for (int c = 0; c < contents.Count; c++)
                {
                    if (contents[c].GetType() == source)
                    {
                        return contents[c];
                    }
                }

                return null;
            }

            public IDatabaseEntry Get(Type source, Type joiner)
            {
                CompositeIdentifier.TypeJoint typeJoint = identifier.GetTypeJoint(source, joiner);
                if (!typeJoint.IsValid)
                {
                    return null;
                }

                IDatabaseEntry sourceData = Get(source);
                for (int c = 0; c < contents.Count; c++)
                {
                    IDatabaseEntry content = contents[c];
                    if (CompositeIdentifier.Match(typeJoint, sourceData, content))
                    {
                        return content;
                    }
                }

                return null;
            }

            public TType[] GetAll<TType>() where TType : IDatabaseEntry
            {
                List<TType> results = new List<TType>();
                for (int c = 0; c < contents.Count; c++)
                {
                    IDatabaseEntry content = contents[c];
                    if (content is TType)
                    {
                        results.Add((TType) content);
                    }
                }

                return results.ToArray();
            }

            public TJoiner[] GetAll<TSource, TJoiner>() where TSource : IDatabaseEntry
                                                        where TJoiner : IDatabaseEntry
            {
                List<TJoiner> results = new List<TJoiner>();
                TSource[] sourceDatas = GetAll<TSource>();
                CompositeIdentifier.TypeJoint typeJoint = identifier.GetTypeJoint(typeof(TSource), typeof(TJoiner));
                for (int s = 0; s < sourceDatas.Length; s++)
                {
                    TSource sourceData = sourceDatas[s];
                    for (int c = 0; c < contents.Count; c++)
                    {
                        IDatabaseEntry content = contents[c];
                        if (!(content is TJoiner))
                        {
                            continue;
                        }

                        if (!CompositeIdentifier.Match(typeJoint, sourceData, content))
                        {
                            continue;
                        }

                        results.Add((TJoiner) contents[c]);
                        break;
                    }
                }

                return results.ToArray();
            }

            public IDatabaseEntry[] GetAll(Type type)
            {
                List<IDatabaseEntry> results = new List<IDatabaseEntry>();
                for (int c = 0; c < contents.Count; c++)
                {
                    if (contents[c].GetType() == type)
                    {
                        results.Add(contents[c]);
                    }
                }

                return results.ToArray();
            }

            public IDatabaseEntry[] GetAll(Type source, Type joiner)
            {
                List<IDatabaseEntry> results = new List<IDatabaseEntry>();
                IDatabaseEntry[] sourceDatas = GetAll(source);
                CompositeIdentifier.TypeJoint typeJoint = identifier.GetTypeJoint(source, joiner);
                for (int s = 0; s < sourceDatas.Length; s++)
                {
                    IDatabaseEntry sourceData = sourceDatas[s];
                    for (int c = 0; c < contents.Count; c++)
                    {
                        IDatabaseEntry content = contents[c];
                        if (!CompositeIdentifier.Match(typeJoint, sourceData, content))
                        {
                            continue;
                        }

                        results.Add(contents[c]);
                        break;
                    }
                }

                return results.ToArray();
            }
            #endregion
        }
        #endregion

        #region Nested type: CompositeIdentifier
        private class CompositeIdentifier : ICompositeIdentifier
        {
            #region Static and Constants
            private static Array IDENTIFIER_STATUS = Enum.GetValues(typeof(IdentifierStatus));
            #endregion

            #region Fields
            public Type rootType;
            public List<TypeJoint> requiredJoints = new List<TypeJoint>();
            public List<TypeJoint> optionalJoints = new List<TypeJoint>();
            #endregion

            #region Constructors
            public CompositeIdentifier(Type rootType)
            {
                this.rootType = rootType;
            }
            #endregion

            #region Class Methods
            public static bool Match(TypeJoint typeJoint, IDatabaseEntry sourceData, IDatabaseEntry joinerData)
            {
                Keyname sourceId = default;
                if (typeJoint.sourceIdFunc == null)
                {
                    sourceId = sourceData.IdUnique;
                }
                else
                {
                    sourceId = typeJoint.sourceIdFunc(sourceData);
                }

                Keyname joinerId = default;
                if (typeJoint.joinerIdFunc == null)
                {
                    joinerId = joinerData.IdUnique;
                }
                else
                {
                    joinerId = typeJoint.joinerIdFunc(joinerData);
                }

                return sourceId == joinerId;
            }

            public TypeJoint GetTypeJoint<TSource, TJoiner>() where TSource : IDatabaseEntry
                                                              where TJoiner : IDatabaseEntry
            {
                return GetTypeJoint(typeof(TSource), typeof(TJoiner));
            }

            public TypeJoint GetTypeJoint(Type source, Type joiner)
            {
                for (int pass = 0; pass <= Consts.SECOND_ITEM; pass++)
                {
                    List<TypeJoint> joints = pass == 0 ? requiredJoints : optionalJoints;
                    int index = joints.FindIndex(x =>
                    {
                        return x.source == source && x.joiner == joiner;
                    });

                    if (index != Consts.INDEX_NONE)
                    {
                        return joints[index];
                    }
                }

                return default;
            }

            public ICompositeContent BuildCompositeContent(Keyname keyname, Dictionary<Type, List<IDatabaseEntry>> databaseEntries)
            {
                CompositeContent composite = null;

#if NVIZZIO_DEV
                using (new ProfilerScope("BuildCompositeContent()"))
#endif
                {
                    if (!databaseEntries.ContainsKey(rootType))
                    {
                        DebugTools.LogError($"Couldn't find {rootType} type of data in the database, this shouldn't happen.");
                        return composite;
                    }

                    composite = new CompositeContent(keyname, this);

                    IDatabaseEntry rootData = databaseEntries[rootType].Find(x => { return x.IdUnique == keyname; });
                    composite.AddContent(rootData);

                    if (RootValidationCondition != null)
                    {
                        // Check if template id of the identifier match with the one set in the basic
                        // infos of the database entry (Basic Infos should be the RootType is this situation)
                        if (!RootValidationCondition(rootData))
                        {
                            return null;
                        }
                    }

                    foreach (IdentifierStatus status in IDENTIFIER_STATUS)
                    {
#if NVIZZIO_DEV
                        using (new ProfilerScope("foreach(IDENTIFIER_STATUS)"))
#endif
                        {
                            List<TypeJoint> joints = status == IdentifierStatus.Required ? requiredJoints : optionalJoints;

                            if (!SearchJoinerTypeInDatabase(joints, databaseEntries, status, composite))
                            {
                                return null;
                            }
                        }
                    }
                }

                return composite;
            }

            private bool SearchJoinerTypeInDatabase(List<TypeJoint> joints, Dictionary<Type, List<IDatabaseEntry>> databaseEntries, IdentifierStatus status, CompositeContent composite)
            {
#if NVIZZIO_DEV
                using (new ProfilerScope("SearchJoinerTypeInDatabase"))
#endif
                {
                    foreach (TypeJoint typeJoint in joints)
                    {
#if NVIZZIO_DEV
                        using (new ProfilerScope("foreach(joints)"))
#endif
                        {
                            if (!databaseEntries.ContainsKey(typeJoint.joiner))
                            {
                                continue;
                            }

                            List<IDatabaseEntry> joinerList = databaseEntries[typeJoint.joiner];

                            if (!TryJoinDatabaseEntryToCompositeContent(joinerList, status, typeJoint, composite))
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }

            private bool TryJoinDatabaseEntryToCompositeContent(List<IDatabaseEntry> joinerList, IdentifierStatus status, TypeJoint typeJoint, CompositeContent composite)
            {
                IDatabaseEntry[] sourceDataArray = composite.GetAll(typeJoint.source);

#if NVIZZIO_DEV
                using (new ProfilerScope("TryJoinDatabaseEntryToCompositeContent"))
#endif
                {
                    foreach (IDatabaseEntry sourceData in sourceDataArray)
                    {
#if NVIZZIO_DEV
                        using (new ProfilerScope("foreach(sourceDataArray)"))
#endif
                        {
                            if (!TryMatchSourceDataWithJoinerEntries(joinerList, status, typeJoint, composite, sourceData))
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }

            private bool TryMatchSourceDataWithJoinerEntries(List<IDatabaseEntry> joinerList, IdentifierStatus status, TypeJoint typeJoint, CompositeContent composite, IDatabaseEntry sourceData)
            {
#if NVIZZIO_DEV
                using (new ProfilerScope("TryMatchSourceDataWithJoinerEntries"))
#endif
                {
                    bool foundAtLeastOne = false;
                    int joinerIndex = 0;
                    while (true)
                    {
#if NVIZZIO_DEV
                        using (new ProfilerScope("while (true)"))
#endif
                        {
                            int foundIndex = joinerList.FindIndex(joinerIndex, joinerData => Match(typeJoint, sourceData, joinerData));
                            if (foundIndex == Consts.INDEX_NONE)
                            {
                                //If the parameter is required, this data is wrong
                                if (!foundAtLeastOne && status == IdentifierStatus.Required)
                                {
                                    // This debug log error is useful because it will break unit tests if an error occurs so don't remove it
                                    DebugTools.LogError($"Composite {composite.Keyname} could not find a data required by the join table '{sourceData.GetType().Name} => {typeJoint.joiner.Name}'");
                                    return false;
                                }

                                break;
                            }

                            foundAtLeastOne = true;
                            composite.AddContent(joinerList[foundIndex]);
                            joinerIndex = foundIndex + Consts.NEXT_ITEM;
                        }
                    }
                }

                return true;
            }

            private bool InternalJoin(
                IdentifierStatus status,
                Type joiner,
                Func<IDatabaseEntry, Keyname> joinerIdFunc,
                Type source,
                Func<IDatabaseEntry, Keyname> sourceIdFunc)
            {
                List<TypeJoint> joints = status == IdentifierStatus.Required ? requiredJoints : optionalJoints;
                if (joints.Contains(joiner))
                {
                    return false;
                }

                joints.Add(new TypeJoint(source, sourceIdFunc, joiner, joinerIdFunc));

                return true;
            }
            #endregion

            #region ICompositeIdentifier Members
            public Func<IDatabaseEntry, bool> RootValidationCondition { get; set; } = null;

            public bool Join<TJoiner>()
            {
                return InternalJoin(IdentifierStatus.Required, typeof(TJoiner), null, rootType, null);
            }

            public bool Join<TJoiner, TSource>()
            {
                return InternalJoin(IdentifierStatus.Required, typeof(TJoiner), null, typeof(TSource), null);
            }

            public bool Join<TJoiner, TSource>(Func<TSource, Keyname> sourceIdFunc)
            {
                Func<IDatabaseEntry, Keyname> sourceTypeFunc = null;
                if (sourceIdFunc != null)
                {
                    sourceTypeFunc = x =>
                    {
                        if (x is TSource)
                        {
                            return sourceIdFunc((TSource) x);
                        }

                        return null;
                    };
                }

                return InternalJoin(IdentifierStatus.Required, typeof(TJoiner), null, typeof(TSource), sourceTypeFunc);
            }

            public bool Join<TJoiner, TSource>(Func<TJoiner, Keyname> joinerIdFunc, Func<TSource, Keyname> sourceIdFunc)
            {
                Func<IDatabaseEntry, Keyname> joinerTypeFunc = null;
                if (joinerIdFunc != null)
                {
                    joinerTypeFunc = x =>
                    {
                        if (x is TJoiner)
                        {
                            return joinerIdFunc((TJoiner) x);
                        }

                        return null;
                    };
                }

                Func<IDatabaseEntry, Keyname> sourceTypeFunc = null;
                if (sourceIdFunc != null)
                {
                    sourceTypeFunc = x =>
                    {
                        if (x is TSource)
                        {
                            return sourceIdFunc((TSource) x);
                        }

                        return null;
                    };
                }

                return InternalJoin(IdentifierStatus.Required, typeof(TJoiner), joinerTypeFunc, typeof(TSource), sourceTypeFunc);
            }

            public bool Join<TJoiner>(IdentifierStatus status)
            {
                return InternalJoin(status, typeof(TJoiner), null, rootType, null);
            }

            public bool Join<TJoiner, TSource>(IdentifierStatus status)
            {
                return InternalJoin(status, typeof(TJoiner), null, typeof(TSource), null);
            }

            public bool Join<TJoiner, TSource>(IdentifierStatus status, Func<TSource, Keyname> sourceIdFunc)
            {
                Func<IDatabaseEntry, Keyname> sourceTypeFunc = null;
                if (sourceIdFunc != null)
                {
                    sourceTypeFunc = x =>
                    {
                        if (x is TSource)
                        {
                            return sourceIdFunc((TSource) x);
                        }

                        return null;
                    };
                }

                return InternalJoin(status, typeof(TJoiner), null, typeof(TSource), sourceTypeFunc);
            }

            public bool Join<TJoiner, TSource>(IdentifierStatus status, Func<TJoiner, Keyname> joinerIdFunc, Func<TSource, Keyname> sourceIdFunc)
            {
                Func<IDatabaseEntry, Keyname> joinerTypeFunc = null;
                if (joinerIdFunc != null)
                {
                    joinerTypeFunc = x =>
                    {
                        if (x is TJoiner)
                        {
                            return joinerIdFunc((TJoiner) x);
                        }

                        return null;
                    };
                }

                Func<IDatabaseEntry, Keyname> sourceTypeFunc = null;
                if (sourceIdFunc != null)
                {
                    sourceTypeFunc = x =>
                    {
                        if (x is TSource)
                        {
                            return sourceIdFunc((TSource) x);
                        }

                        return null;
                    };
                }

                return InternalJoin(status, typeof(TJoiner), joinerTypeFunc, typeof(TSource), sourceTypeFunc);
            }
            #endregion

            #region Nested type: TypeJoint
            public struct TypeJoint
            {
                public Type source;
                public Type joiner;
                public Func<IDatabaseEntry, Keyname> sourceIdFunc;
                public Func<IDatabaseEntry, Keyname> joinerIdFunc;

                public bool IsValid
                {
                    get { return source != null && joiner != null; }
                }

                public TypeJoint(Type source, Func<IDatabaseEntry, Keyname> sourceIdFunc, Type joiner,
                                 Func<IDatabaseEntry, Keyname> joinerIdFunc)
                {
                    this.source = source;
                    this.joiner = joiner;
                    this.sourceIdFunc = sourceIdFunc;
                    this.joinerIdFunc = joinerIdFunc;
                }

                public static implicit operator TypeJoint(Type joiner)
                {
                    return new TypeJoint(null, null, joiner, null);
                }

                public override int GetHashCode()
                {
                    return joiner.GetHashCode();
                }
            }
            #endregion
        }
        #endregion
    }
}
