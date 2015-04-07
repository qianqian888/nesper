///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

using com.espertech.esper.client;
using com.espertech.esper.client.context;
using com.espertech.esper.compat.collections;
using com.espertech.esper.core.context.stmt;
using com.espertech.esper.core.service;
using com.espertech.esper.epl.spec;
using com.espertech.esper.filter;
using com.espertech.esper.pattern;
using com.espertech.esper.schedule;

namespace com.espertech.esper.core.context.mgr
{
    public class ContextControllerInitTermFactory : ContextControllerFactoryBase, ContextControllerFactory
    {
        private readonly ContextDetailInitiatedTerminated _detail;
        private readonly ContextStateCache _stateCache;
    
        private readonly ContextStatePathValueBinding _binding;
    
        private IDictionary<String, Object> _contextBuiltinProps;
        private MatchedEventMapMeta _matchedEventMapMeta;
    
        public ContextControllerInitTermFactory(ContextControllerFactoryContext factoryContext, ContextDetailInitiatedTerminated detail, ContextStateCache stateCache)
                    : base(factoryContext)
        {
            _detail = detail;
            _stateCache = stateCache;
            _binding = stateCache.GetBinding(detail);
        }
    
        public override void ValidateFactory()
        {
            _contextBuiltinProps = ContextPropertyEventType.GetInitiatedTerminatedType();
            LinkedHashSet<String> allTags = new LinkedHashSet<String>();
            ContextPropertyEventType.AddEndpointTypes(FactoryContext.ContextName, _detail.Start, _contextBuiltinProps, allTags);
            ContextPropertyEventType.AddEndpointTypes(FactoryContext.ContextName, _detail.End, _contextBuiltinProps, allTags);
            _matchedEventMapMeta = new MatchedEventMapMeta(allTags, false);
        }

        public override ContextStateCache StateCache
        {
            get { return _stateCache; }
        }

        public ContextStatePathValueBinding Binding
        {
            get { return _binding; }
        }

        public override IDictionary<string, object> ContextBuiltinProps
        {
            get { return _contextBuiltinProps; }
        }

        public MatchedEventMapMeta MatchedEventMapMeta
        {
            get { return _matchedEventMapMeta; }
        }

        public override ContextControllerStatementCtxCache ValidateStatement(ContextControllerStatementBase statement)
        {
            return null;
        }
    
        public override ContextController CreateNoCallback(int pathId, ContextControllerLifecycleCallback callback)
        {
            return new ContextControllerInitTerm(pathId, callback, this);
        }

        public override void PopulateFilterAddendums(IDictionary<FilterSpecCompiled, FilterValueSetParam[][]> filterAddendum, ContextControllerStatementDesc statement, object key, int contextId)
        {
        }

        public override FilterSpecLookupable GetFilterLookupable(EventType eventType)
        {
            return null;
        }

        public override ContextDetail ContextDetail
        {
            get { return _detail; }
        }

        public ContextDetailInitiatedTerminated ContextDetailInitiatedTerminated
        {
            get { return _detail; }
        }

        public override IList<ContextDetailPartitionItem> ContextDetailPartitionItems
        {
            get { return Collections.GetEmptyList<ContextDetailPartitionItem>(); }
        }

        public override StatementAIResourceRegistryFactory StatementAIResourceRegistryFactory
        {
            get
            {
                if (_detail.IsOverlapping)
                {
                    return
                        () =>
                            new StatementAIResourceRegistry(new AIRegistryAggregationMultiPerm(), new AIRegistryExprMultiPerm());
                }
                else
                {
                    return () => new StatementAIResourceRegistry(new AIRegistryAggregationSingle(), new AIRegistryExprSingle());
                }
            }
        }

        public override bool IsSingleInstanceContext
        {
            get { return !_detail.IsOverlapping; }
        }

        public ScheduleSlot AllocateSlot()
        {
            return FactoryContext.AgentInstanceContextCreate.StatementContext.ScheduleBucket.AllocateSlot();
        }

        public TimeProvider TimeProvider
        {
            get { return FactoryContext.AgentInstanceContextCreate.StatementContext.TimeProvider; }
        }

        public SchedulingService SchedulingService
        {
            get { return FactoryContext.AgentInstanceContextCreate.StatementContext.SchedulingService; }
        }

        public EPStatementHandle EpStatementHandle
        {
            get { return FactoryContext.AgentInstanceContextCreate.StatementContext.EpStatementHandle; }
        }

        public StatementContext StatementContext
        {
            get { return FactoryContext.AgentInstanceContextCreate.StatementContext; }
        }

        public override ContextPartitionIdentifier KeyPayloadToIdentifier(Object payload)
        {
            var state = (ContextControllerInitTermState) payload;
            return new ContextPartitionIdentifierInitiatedTerminated(
                    state == null ? null : state.PatternData,
                    state == null ? 0 : state.StartTime,
                    null);
        }
    }
}
