///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using NUnit.Framework;


namespace com.espertech.esper.filter
{
    [TestFixture]
    public class TestRangeValueDouble 
    {
        private readonly FilterSpecParamRangeValue[] _params = new FilterSpecParamRangeValue[5];
    
        [SetUp]
        public void SetUp()
        {
            _params[0] = new RangeValueDouble(5.5);
            _params[1] = new RangeValueDouble(0);
            _params[2] = new RangeValueDouble(5.5);
        }
    
        [Test]
        public void TestGetFilterValue()
        {
            Assert.AreEqual(5.5, _params[0].GetFilterValue(null, null));
        }
    
        [Test]
        public void TestEquals()
        {
            Assert.IsFalse(_params[0].Equals(_params[1]));
            Assert.IsFalse(_params[1].Equals(_params[2]));
            Assert.IsTrue(_params[0].Equals(_params[2]));
        }
    }
}
