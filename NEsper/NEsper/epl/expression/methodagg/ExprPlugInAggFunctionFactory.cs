///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

using com.espertech.esper.client;
using com.espertech.esper.client.hook;
using com.espertech.esper.compat;
using com.espertech.esper.compat.collections;
using com.espertech.esper.epl.agg.access;
using com.espertech.esper.epl.agg.aggregator;
using com.espertech.esper.epl.agg.service;
using com.espertech.esper.epl.core;
using com.espertech.esper.epl.expression.baseagg;
using com.espertech.esper.epl.expression.core;

namespace com.espertech.esper.epl.expression.methodagg
{
    [Serializable]
	public class ExprPlugInAggFunctionFactory : AggregationMethodFactory
	{
	    private readonly ExprPlugInAggFunctionFactoryNode _parent;
	    private readonly AggregationFunctionFactory _aggregationFunctionFactory;
	    private readonly Type _aggregatedValueType;

	    public ExprPlugInAggFunctionFactory(ExprPlugInAggFunctionFactoryNode parent, AggregationFunctionFactory aggregationFunctionFactory, Type aggregatedValueType) {
	        this._parent = parent;
	        this._aggregationFunctionFactory = aggregationFunctionFactory;
	        this._aggregatedValueType = aggregatedValueType;
	    }

        public Type ResultType
        {
            get { return _aggregationFunctionFactory.ValueType; }
        }

        public bool IsAccessAggregation
        {
            get { return false; }
        }

        public AggregationStateKey GetAggregationStateKey(bool isMatchRecognize) {
	        throw new IllegalStateException("Not an access aggregation function");
	    }

	    public AggregationStateFactory GetAggregationStateFactory(bool isMatchRecognize) {
	        throw new IllegalStateException("Not an access aggregation function");
	    }

        public AggregationAccessor Accessor
        {
            get { throw new IllegalStateException("Not an access aggregation function"); }
        }

        public AggregationMethod Make(MethodResolutionService methodResolutionService, int agentInstanceId, int groupId, int aggregationId) {

	        AggregationMethod method = _aggregationFunctionFactory.NewAggregator();
	        if (!_parent.IsDistinct) {
	            return method;
	        }
	        return methodResolutionService.MakeDistinctAggregator(agentInstanceId, groupId, aggregationId, method, _aggregatedValueType,false);
	    }

        public ExprAggregateNodeBase AggregationExpression
        {
            get { return _parent; }
        }

        public void ValidateIntoTableCompatible(AggregationMethodFactory intoTableAgg) {
	        AggregationMethodFactoryUtil.ValidateAggregationType(this, intoTableAgg);
	    }

        public AggregationAgent AggregationStateAgent
        {
            get { return null; }
        }

        public ExprEvaluator GetMethodAggregationEvaluator(bool join, EventType[] typesPerStream)
        {
            return ExprMethodAggUtil.GetDefaultEvaluator(_parent.PositionalParams, join, typesPerStream);
        }
	}
} // end of namespace
