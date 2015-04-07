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
using com.espertech.esper.compat;
using com.espertech.esper.compat.collections;
using com.espertech.esper.epl.expression.core;
using com.espertech.esper.epl.expression.table;

namespace com.espertech.esper.core.context.stmt
{
    public class AIRegistryTableAccessMap : AIRegistryTableAccess, ExprTableAccessEvalStrategy
    {
        private readonly IDictionary<int, ExprTableAccessEvalStrategy> strategies;
    
        public AIRegistryTableAccessMap()
        {
            strategies = new Dictionary<int, ExprTableAccessEvalStrategy>();
        }
    
        public void AssignService(int num, ExprTableAccessEvalStrategy value)
        {
            strategies.Put(num, value);
        }
    
        public void DeassignService(int num)
        {
            strategies.Remove(num);
        }

        public int AgentInstanceCount
        {
            get { return strategies.Count; }
        }

        public object Evaluate(EventBean[] eventsPerStream, bool isNewData, ExprEvaluatorContext context)
        {
            return GetStrategy(context).Evaluate(eventsPerStream, isNewData, context);
        }
    
        public object[] EvaluateTypableSingle(EventBean[] eventsPerStream, bool isNewData, ExprEvaluatorContext context)
        {
            return GetStrategy(context).EvaluateTypableSingle(eventsPerStream, isNewData, context);
        }
    
        public ICollection<EventBean> EvaluateGetROCollectionEvents(EventBean[] eventsPerStream, bool isNewData, ExprEvaluatorContext context)
        {
            return GetStrategy(context).EvaluateGetROCollectionEvents(eventsPerStream, isNewData, context);
        }
    
        public EventBean EvaluateGetEventBean(EventBean[] eventsPerStream, bool isNewData, ExprEvaluatorContext context)
        {
            return GetStrategy(context).EvaluateGetEventBean(eventsPerStream, isNewData, context);
        }
    
        public ICollection<object> EvaluateGetROCollectionScalar(EventBean[] eventsPerStream, bool isNewData, ExprEvaluatorContext context)
        {
            return GetStrategy(context).EvaluateGetROCollectionScalar(eventsPerStream, isNewData, context);
        }
    
        private ExprTableAccessEvalStrategy GetStrategy(ExprEvaluatorContext context)
        {
            return strategies.Get(context.AgentInstanceId);
        }
    }
}
