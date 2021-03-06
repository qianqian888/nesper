///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using com.espertech.esper.client;
using com.espertech.esper.compat;
using com.espertech.esper.epl.datetime.eval;
using com.espertech.esper.epl.expression.core;
using com.espertech.esper.epl.expression;
using com.espertech.esper.epl.expression.dot;

namespace com.espertech.esper.epl.datetime.reformatop
{
    public class ReformatOpToMillisec : ReformatOp
    {
        public Object Evaluate(long ts, EventBean[] eventsPerStream, bool newData, ExprEvaluatorContext exprEvaluatorContext)
        {
            return ts;
        }

        public object Evaluate(DateTimeOffset d, EventBean[] eventsPerStream, bool newData, ExprEvaluatorContext exprEvaluatorContext)
        {
            return d.TimeInMillis();
        }

        public Type ReturnType
        {
            get { return typeof(long?); }
        }

        public ExprDotNodeFilterAnalyzerDesc GetFilterDesc(EventType[] typesPerStream,
                                           DatetimeMethodEnum currentMethod,
                                           ICollection<ExprNode> currentParameters,
                                           ExprDotNodeFilterAnalyzerInput inputDesc)
        {
            return null;
        }
    }
}
