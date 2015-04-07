///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

using com.espertech.esper.client.annotation;
using com.espertech.esper.compat.threading;
using com.espertech.esper.epl.annotation;

namespace com.espertech.esper.core.service
{
    /// <summary>
    /// Provides statement-level locks.
    /// </summary>
    public class StatementLockFactoryImpl : StatementLockFactory
    {
        private readonly bool _fairlocks;
        private readonly bool _disableLocking;
    
        public StatementLockFactoryImpl(bool fairlocks, bool disableLocking)
        {
            _fairlocks = fairlocks;
            _disableLocking = disableLocking;
        }
    
        public IReaderWriterLock GetStatementLock(String statementName, Attribute[] annotations, bool stateless)
        {
            bool foundNoLock = AnnotationUtil.FindAttribute(annotations, typeof(NoLockAttribute)) != null;
            if (_disableLocking || foundNoLock || stateless)
            {
                return ReaderWriterLockManager.VoidLock();
            }

            if (_fairlocks)
            {
                return ReaderWriterLockManager.FairLock();
            }

            return ReaderWriterLockManager.CreateDefaultLock();
        }
    }
}
