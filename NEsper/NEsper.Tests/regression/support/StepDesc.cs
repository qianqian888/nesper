///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

namespace com.espertech.esper.regression.support
{
    public class StepDesc
    {
        private readonly int step;
        private readonly Object[][] newDataPerRow;
        private readonly Object[][] oldDataPerRow;
    
        public StepDesc(int step, Object[][] newDataPerRow, Object[][] oldDataPerRow) {
            this.step = step;
            this.newDataPerRow = newDataPerRow;
            this.oldDataPerRow = oldDataPerRow;
        }

        public int Step
        {
            get { return step; }
        }

        public object[][] NewDataPerRow
        {
            get { return newDataPerRow; }
        }

        public object[][] OldDataPerRow
        {
            get { return oldDataPerRow; }
        }
    }
}
