///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using com.espertech.esper.collection;
using com.espertech.esper.epl.agg.access;
using com.espertech.esper.epl.agg.aggregator;
using com.espertech.esper.epl.agg.service;
using com.espertech.esper.epl.core;
using com.espertech.esper.events;
using com.espertech.esper.events.arr;

namespace com.espertech.esper.epl.table.mgmt
{
    public class TableStateRowFactory
    {
        private readonly ObjectArrayEventType _objectArrayEventType;
        private readonly MethodResolutionService _methodResolutionService;
        private readonly AggregationMethodFactory[] _methodFactories;
        private readonly AggregationStateFactory[] _stateFactories;
        private readonly int[] _groupKeyIndexes;
        private readonly EventAdapterService _eventAdapterService;
    
        public TableStateRowFactory(ObjectArrayEventType objectArrayEventType, MethodResolutionService methodResolutionService, AggregationMethodFactory[] methodFactories, AggregationStateFactory[] stateFactories, int[] groupKeyIndexes, EventAdapterService eventAdapterService)
        {
            _objectArrayEventType = objectArrayEventType;
            _methodResolutionService = methodResolutionService;
            _methodFactories = methodFactories;
            _stateFactories = stateFactories;
            _groupKeyIndexes = groupKeyIndexes;
            _eventAdapterService = eventAdapterService;
        }
    
        public ObjectArrayBackedEventBean MakeOA(int agentInstanceId, object groupByKey, object groupKeyBinding) {
            var row = MakeAggs(agentInstanceId, groupByKey, groupKeyBinding);
            var data = new object[_objectArrayEventType.PropertyDescriptors.Count];
            data[0] = row;
    
            if (_groupKeyIndexes.Length == 1) {
                data[_groupKeyIndexes[0]] = groupByKey;
            }
            else {
                if (_groupKeyIndexes.Length > 1) {
                    object[] keys = ((MultiKeyUntyped) groupByKey).Keys;
                    for (int i = 0; i < _groupKeyIndexes.Length; i++) {
                        data[_groupKeyIndexes[i]] = keys[i];
                    }
                }
            }
    
            return (ObjectArrayBackedEventBean) _eventAdapterService.AdapterForType(data, _objectArrayEventType);
        }
    
        public AggregationRowPair MakeAggs(int agentInstanceId, object groupByKey, object groupKeyBinding) {
            AggregationMethod[] methods = _methodResolutionService.NewAggregators(_methodFactories, agentInstanceId, groupByKey, groupKeyBinding, null);
            AggregationState[] states = _methodResolutionService.NewAccesses(agentInstanceId, false, _stateFactories, groupByKey, groupKeyBinding, null);   return new AggregationRowPair(methods, states);
        }
    }
}
