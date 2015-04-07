///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

using com.espertech.esper.client;
using com.espertech.esper.epl.expression;
using com.espertech.esper.epl.expression.core;

namespace com.espertech.esper.epl.core.eval
{
    public class EvalInsertUtil
    {
        public static ExprValidationException MakeEventTypeCastException(EventType sourceType, EventType targetType) {
            return new ExprValidationException("Expression-returned event type '" + sourceType.Name +
                        "' with underlying type '" + sourceType.UnderlyingType.FullName +
                        "' cannot be converted to target event type '" + targetType.Name +
                        "' with underlying type '" + targetType.UnderlyingType.FullName + "'");
        }
    
        public static ExprValidationException MakeEventTypeCastException(Type sourceType, EventType targetType) {
            return new ExprValidationException("Expression-returned value of type '" + sourceType.FullName +
                    "' cannot be converted to target event type '" + targetType.Name +
                    "' with underlying type '" + targetType.UnderlyingType.FullName + "'");
        }
    }
}
