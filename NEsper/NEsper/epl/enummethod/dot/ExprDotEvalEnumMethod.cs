///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

using com.espertech.esper.epl.expression;
using com.espertech.esper.epl.expression.core;
using com.espertech.esper.epl.expression.dot;
using com.espertech.esper.epl.rettype;

namespace com.espertech.esper.epl.enummethod.dot
{
    public interface ExprDotEvalEnumMethod : ExprDotEval
    {
        void Init(
            int? streamOfProviderIfApplicable,
            EnumMethodEnum lambda,
            String lambdaUsedName,
            EPType currentInputType,
            IList<ExprNode> parameters,
            ExprValidationContext validationContext);
    }
}
