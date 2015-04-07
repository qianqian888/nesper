///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

using com.espertech.esper.compat;
using com.espertech.esper.compat.collections;
using com.espertech.esper.epl.table.mgmt;
using com.espertech.esper.util;

namespace com.espertech.esper.core.start
{
    public class EPStatementDestroyCallbackTableUpdStr : DestroyCallback
    {
        private readonly TableService tableService;
        private readonly TableMetadata tableMetadata;
        private readonly string statementName;
    
        public EPStatementDestroyCallbackTableUpdStr(TableService tableService, TableMetadata tableMetadata, string statementName) {
            this.tableService = tableService;
            this.tableMetadata = tableMetadata;
            this.statementName = statementName;
        }
    
        public void Destroy() {
            tableService.RemoveTableUpdateStrategyReceivers(tableMetadata, statementName);
        }
    }
}
