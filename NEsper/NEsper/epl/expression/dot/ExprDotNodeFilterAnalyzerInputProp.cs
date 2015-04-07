///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

namespace com.espertech.esper.epl.expression.dot
{
    public class ExprDotNodeFilterAnalyzerInputProp : ExprDotNodeFilterAnalyzerInput
    {
        public ExprDotNodeFilterAnalyzerInputProp(int streamNum,
                                                  String propertyName)
        {
            StreamNum = streamNum;
            PropertyName = propertyName;
        }

        public int StreamNum { get; private set; }

        public string PropertyName { get; private set; }
    }
}