///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

using com.espertech.esper.epl.core;
using com.espertech.esper.epl.datetime.eval;
using com.espertech.esper.epl.expression.core;
using com.espertech.esper.epl.expression;

namespace com.espertech.esper.epl.datetime.interval
{
    public class IntervalOpFactory : OpFactory
    {
        public IntervalOp GetOp(StreamTypeService streamTypeService, DatetimeMethodEnum method, String methodNameUsed, IList<ExprNode> parameters, ExprEvaluator[] evaluators)
        {
            return new IntervalOpImpl(method, methodNameUsed, streamTypeService, parameters);
        }
    }
}
