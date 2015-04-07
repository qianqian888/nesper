///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

using com.espertech.esper.epl.expression.core;

namespace com.espertech.esper.epl.expression.baseagg
{
	[Serializable]
    public class ExprAggregateLocalGroupByDesc
    {
	    public ExprAggregateLocalGroupByDesc(ExprNode[] partitionExpressions)
        {
	        PartitionExpressions = partitionExpressions;
	    }

	    public ExprNode[] PartitionExpressions { get; private set; }
    }
} // end of namespace
