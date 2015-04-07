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
using com.espertech.esper.compat.threading;
using com.espertech.esper.epl.join.exec.@base;
using com.espertech.esper.epl.join.table;
using com.espertech.esper.epl.virtualdw;
using com.espertech.esper.util;
using com.espertech.esper.view;

namespace com.espertech.esper.epl.join.plan
{
    public class QueryPlanNodeNoOp : QueryPlanNode
    {
        private static readonly ExecNodeNoOp NOOP = new ExecNodeNoOp();

        public override ExecNode MakeExec(string statementName, string statementId, Attribute[] annotations, IDictionary<TableLookupIndexReqKey, EventTable>[] indexesPerStream, EventType[] streamTypes, Viewable[] streamViews, HistoricalStreamIndexList[] historicalStreamIndexLists, VirtualDWView[] viewExternal, ILockable[] tableSecondaryIndexLocks)
        {
            return NOOP;
        }

        public override void AddIndexes(HashSet<TableLookupIndexReqKey> usedIndexes)
        {
        }

        protected internal override void Print(IndentWriter writer)
        {
            writer.WriteLine("No-Op Execution");
        }
    }
}
