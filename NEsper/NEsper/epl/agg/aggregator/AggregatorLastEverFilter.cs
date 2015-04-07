///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////


using System;

using com.espertech.esper.epl.agg.service;

namespace com.espertech.esper.epl.agg.aggregator
{
    /// <summary>
    /// Aggregator for the very last value.
    /// </summary>
    public class AggregatorLastEverFilter : AggregatorLastEver
    {
        public AggregatorLastEverFilter(Type type)
            : base(type)
        {
        }
    
        public override void Enter(Object parameters)
        {
            var paramArray = (Object[]) parameters;
            if (!AggregatorUtil.CheckFilter(paramArray)) {
                return;
            }
            base.Enter(paramArray[0]);
        }
    }
}
