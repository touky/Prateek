namespace Mayfair.Core.Code.Database
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Mayfair.Core.Code.Database.Interfaces;
    using Mayfair.Core.Code.Database.Messages;
    using Mayfair.Core.Code.Database.Messages.RequestFilters;
    using Mayfair.Core.Code.Database.ServiceProvider;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Debug;
    using Prateek.Runtime.AppContentFramework.Daemons;
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.KeynameFramework;
    using Prateek.Runtime.KeynameFramework.Enums;
    using Prateek.Runtime.TickableFramework.Interfaces;
    using DatabaseContentByFilterRequest = Mayfair.Core.Code.Database.Messages.DatabaseContentMatchingWithFilterRequest<Messages.DatabaseContentMatchingWithFilterResponse>;

    public sealed class DatabaseDaemonOverseer
        : ContentAccessDaemonOverseer<DatabaseDaemonOverseer, DatabaseServant>
        , IDebugMenuNotebookOwner
        , IPreUpdateTickable
    {
        #region DatabaseRebuildStatus enum
        private enum DatabaseRebuildStatus
        {
            Ready,
            IsRebuilding,
            ShouldRebuild
        }
        #endregion

        #region IdentifierRequestStatus enum
        private enum IdentifierRequestStatus
        {
            NotInitialized,
            WaitingForIdentifiers,
            Received
        }
        #endregion

        #region Fields
        private Dictionary<Type, List<IDatabaseEntry>> databaseEntries = new Dictionary<Type, List<IDatabaseEntry>>();
        private HashSet<CompositeIdentifier> identifiers = new HashSet<CompositeIdentifier>();
        private Queue<CompositeIdentifier> identifiersToRebuild = new Queue<CompositeIdentifier>();

        private readonly Dictionary<Keyname, ICompositeContent> allCompositeContents = new Dictionary<Keyname, ICompositeContent>();

        private readonly Queue<DatabaseContentByKeynameRequest> requestsForContentHold = new Queue<DatabaseContentByKeynameRequest>();
        private readonly Queue<DatabaseContentByFilterRequest> requestsWithFilterHold = new Queue<DatabaseContentByFilterRequest>();

        private IdentifierRequestStatus identifierStatus = IdentifierRequestStatus.NotInitialized;
        private ResourcesLoadingStatus resourcesLoadingStatus = ResourcesLoadingStatus.NotStarted;

        private DatabaseRebuildStatus rebuildStatus;
        #endregion

        #region Register/Unregister
        protected override void OnAwake()
        {
            SetupDebugContent();
        }
        #endregion

        #region Class Methods
        private void OnIdentifiersReceived(DatabaseIdentifierResponse response)
        {
            foreach (var identifier in response.Identifiers)
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
                foreach (var identifier in identifiers)
                {
                    identifiersToRebuild.Enqueue(identifier);
                }
            }
        }

        private void OnRequestContentById(DatabaseContentByKeynameRequest request)
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

            var response = request.GetResponse<DatabaseContentByKeynameResponse>();

            for (int i = 0, n = request.RequestedKeynames.Count; i < n; i++)
            {
                var keyname = request.RequestedKeynames[i];

                //Name can only match as equal, so directly try-get
                if (keyname.State == KeynameState.Fullname)
                {
                    if (allCompositeContents.TryGetValue(keyname, out var compositeContentFound))
                    {
                        response.Content.Add(compositeContentFound);
                    }
                }

                //For a filter, use the IdMatchRequirement to ensure it's correctly matched
                else if (keyname.State == KeynameState.Keywords)
                {
                    foreach (var content in allCompositeContents)
                    {
                        if (keyname.Match(content.Key) <= request.MatchRequirement)
                        {
                            response.Content.Add(content.Value);
                        }
                    }
                }
            }

            CommandReceiver.Send(response);
        }

        private void SendRequestForIdentifiers()
        {
            if (!identifierStatus.Equals(IdentifierRequestStatus.WaitingForIdentifiers))
            {
                identifierStatus = IdentifierRequestStatus.WaitingForIdentifiers;
                var notice =
                    CommandHelper.Create<DatabaseIdentifierRequest<DatabaseIdentifierResponse>>();

                CommandReceiver.Send(notice);
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

            var response = request.GetResponse<DatabaseContentByKeynameResponse>();

            if (request.Operator == FilterLogicalOperators.AND)
            {
                //todo PatternContainsAll(request.Filters, response.Content);
            }
            else
            {
                //todo PatternContainsAny(request.Filters, response.Content);
            }

            DebugTools.Log(this, "OnDatabaseContentMatchingFilter request received and handled. Sending result now");

            CommandReceiver.Send(response);
        }

        private void PatternContainsAll(List<string> filters, List<ICompositeContent> results)
        {
            //todo: foreach (var pair in allCompositeContents)
            //todo: {
            //todo:     var valid            = true;
            //todo:     var uniqueIdAsString = pair.Key.RawValue;
            //todo:     for (int i = 0, n = filters.Count; i < n; i++)
            //todo:     {
            //todo:         if (!uniqueIdAsString.Contains(filters[i]))
            //todo:         {
            //todo:             valid = false;
            //todo:             break;
            //todo:         }
            //todo:     }
            //todo:
            //todo:     if (valid)
            //todo:     {
            //todo:         results.Add(pair.Value);
            //todo:     }
            //todo: }
        }

        private void PatternContainsAny(List<string> filters, List<ICompositeContent> results)
        {
            //todo: foreach (var pair in allCompositeContents)
            //todo: {
            //todo:     var valid            = false;
            //todo:     var uniqueIdAsString = pair.Key.RawValue;
            //todo:     for (int i = 0, n = filters.Count; i < n; i++)
            //todo:     {
            //todo:         if (uniqueIdAsString.Contains(filters[i]))
            //todo:         {
            //todo:             valid = true;
            //todo:             break;
            //todo:         }
            //todo:     }
            //todo: 
            //todo:     if (valid)
            //todo:     {
            //todo:         results.Add(pair.Value);
            //todo:     }
            //todo: }
        }

        private void RefreshPendingResources()
        {
            var servant = FirstAliveServant;
            if (servant == null)
            {
                return;
            }

            servant.RefreshPendingResources(this);
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
            var composite = identifier as CompositeIdentifier;
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

#if PRATEEK_DEBUG
            using (new ProfilerScope("RefreshContentRebuild()"))
#endif
            {
                while (true)

                    //foreach (CompositeIdentifier identifier in identifiers)
                {
                    var identifier = identifiersToRebuild.Dequeue();
#if PRATEEK_DEBUG
                    using (new ProfilerScope("foreach(identifiers)"))
#endif
                    {
                        if (!databaseEntries.TryGetValue(identifier.rootType, out var dataEntries))
                        {
                            //On fail, continue with the next identifier
                            continue;
                        }

#if PRATEEK_DEBUG
                        using (new ProfilerScope($"using {identifier.rootType.Name}"))
#endif
                        {
                            foreach (var dataEntry in dataEntries)
                            {
#if PRATEEK_DEBUG
                                using (new ProfilerScope("foreach(dataEntries)"))
#endif
                                {
                                    var content = identifier.BuildCompositeContent(dataEntry.IdUnique, databaseEntries);
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

        [Conditional("PRATEEK_DEBUG")]
        public void SetupDebugContent()
        {
            var debugNotebook = new DebugMenuNotebook("DTBS", "Database Service");

            var main = new EmptyMenuPage("MAIN");
            debugNotebook.AddPagesWithParent(main, new DatabaseEntryMenuPage(this, "Database entries"));
            debugNotebook.Register();
        }

        #region Messaging
        public override void DefineCommandReceiverActions()
        {
            base.DefineCommandReceiverActions();

            CommandReceiver.SetActionFor<DatabaseIdentifierResponse>(OnIdentifiersReceived);
            CommandReceiver.SetActionFor<DatabaseContentByKeynameRequest>(OnRequestContentById);
            CommandReceiver.SetActionFor<DatabaseContentByFilterRequest>(OnDatabaseContentMatchingFilter);
        }
        #endregion
        #endregion

        #region IPreUpdateTickable Members
        public void PreUpdate()
        {
            RefreshPendingResources();

            if (rebuildStatus > DatabaseRebuildStatus.Ready)
            {
                RefreshContentRebuild();
            }
        }
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
                for (var c = 0; c < contents.Count; c++)
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
                var typeJoint = identifier.GetTypeJoint(source, joiner);
                if (!typeJoint.IsValid)
                {
                    return null;
                }

                var sourceData = Get(source);
                for (var c = 0; c < contents.Count; c++)
                {
                    var content = contents[c];
                    if (CompositeIdentifier.Match(typeJoint, sourceData, content))
                    {
                        return content;
                    }
                }

                return null;
            }

            public TType[] GetAll<TType>() where TType : IDatabaseEntry
            {
                var results = new List<TType>();
                for (var c = 0; c < contents.Count; c++)
                {
                    var content = contents[c];
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
                var results     = new List<TJoiner>();
                var sourceDatas = GetAll<TSource>();
                var typeJoint   = identifier.GetTypeJoint(typeof(TSource), typeof(TJoiner));
                for (var s = 0; s < sourceDatas.Length; s++)
                {
                    var sourceData = sourceDatas[s];
                    for (var c = 0; c < contents.Count; c++)
                    {
                        var content = contents[c];
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
                var results = new List<IDatabaseEntry>();
                for (var c = 0; c < contents.Count; c++)
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
                var results     = new List<IDatabaseEntry>();
                var sourceDatas = GetAll(source);
                var typeJoint   = identifier.GetTypeJoint(source, joiner);
                for (var s = 0; s < sourceDatas.Length; s++)
                {
                    var sourceData = sourceDatas[s];
                    for (var c = 0; c < contents.Count; c++)
                    {
                        var content = contents[c];
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
                for (var pass = 0; pass <= Consts.SECOND_ITEM; pass++)
                {
                    var joints = pass == 0 ? requiredJoints : optionalJoints;
                    var index = joints.FindIndex(x =>
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

#if PRATEEK_DEBUG
                using (new ProfilerScope("BuildCompositeContent()"))
#endif
                {
                    if (!databaseEntries.ContainsKey(rootType))
                    {
                        DebugTools.LogError($"Couldn't find {rootType} type of data in the database, this shouldn't happen.");
                        return composite;
                    }

                    composite = new CompositeContent(keyname, this);

                    var rootData = databaseEntries[rootType].Find(x => { return x.IdUnique == keyname; });
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
#if PRATEEK_DEBUG
                        using (new ProfilerScope("foreach(IDENTIFIER_STATUS)"))
#endif
                        {
                            var joints = status == IdentifierStatus.Required ? requiredJoints : optionalJoints;

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
#if PRATEEK_DEBUG
                using (new ProfilerScope("SearchJoinerTypeInDatabase"))
#endif
                {
                    foreach (var typeJoint in joints)
                    {
#if PRATEEK_DEBUG
                        using (new ProfilerScope("foreach(joints)"))
#endif
                        {
                            if (!databaseEntries.ContainsKey(typeJoint.joiner))
                            {
                                continue;
                            }

                            var joinerList = databaseEntries[typeJoint.joiner];

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
                var sourceDataArray = composite.GetAll(typeJoint.source);

#if PRATEEK_DEBUG
                using (new ProfilerScope("TryJoinDatabaseEntryToCompositeContent"))
#endif
                {
                    foreach (var sourceData in sourceDataArray)
                    {
#if PRATEEK_DEBUG
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
#if PRATEEK_DEBUG
                using (new ProfilerScope("TryMatchSourceDataWithJoinerEntries"))
#endif
                {
                    var foundAtLeastOne = false;
                    var joinerIndex     = 0;
                    while (true)
                    {
#if PRATEEK_DEBUG
                        using (new ProfilerScope("while (true)"))
#endif
                        {
                            var foundIndex = joinerList.FindIndex(joinerIndex, joinerData => Match(typeJoint, sourceData, joinerData));
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
                var joints = status == IdentifierStatus.Required ? requiredJoints : optionalJoints;
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
