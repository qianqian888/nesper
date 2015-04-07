///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

using com.espertech.esper.epl.agg.service;
using com.espertech.esper.epl.expression.baseagg;
using com.espertech.esper.epl.expression.core;

namespace com.espertech.esper.epl.expression.methodagg
{
    /// <summary>
    /// Represents the Avg(...) aggregate function is an expression tree.
    /// </summary>
    [Serializable]
    public class ExprAvgNode : ExprAggregateNodeBase
    {
        private bool _hasFilter;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="distinct">flag indicating unique or non-unique value aggregation</param>
        public ExprAvgNode(bool distinct)
            : base(distinct)
        {
        }
    
        public override AggregationMethodFactory ValidateAggregationChild(ExprValidationContext validationContext)
        {
            _hasFilter = PositionalParams.Length > 1;
            var childType = base.ValidateNumericChildAllowFilter(_hasFilter);
            return new ExprAvgNodeFactory(this, childType, validationContext.MethodResolutionService);
        }

        public override string AggregationFunctionName
        {
            get { return "avg"; }
        }

        protected override bool EqualsNodeAggregateMethodOnly(ExprAggregateNode node)
        {
            return node is ExprAvgNode;
        }

        public bool HasFilter
        {
            get { return _hasFilter; }
        }
    }
}
