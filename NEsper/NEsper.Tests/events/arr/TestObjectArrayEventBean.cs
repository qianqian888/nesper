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
using com.espertech.esper.compat.collections;
using com.espertech.esper.compat.logging;
using com.espertech.esper.support.bean;
using com.espertech.esper.support.events;

using NUnit.Framework;

namespace com.espertech.esper.events.arr
{
    [TestFixture]
    public class TestObjectArrayEventBean 
    {
        private String[] _testProps;
        private Object[] _testTypes;
        private Object[] _testValues;
    
        private EventType _eventType;
        private ObjectArrayEventBean _eventBean;
    
        private readonly SupportBeanComplexProps _supportBean = SupportBeanComplexProps.MakeDefaultBean();
    
        [SetUp]
        public void SetUp()
        {
            _testProps = new String[] {"aString", "anInt", "myComplexBean"};
            _testTypes = new Object[] {typeof(String), typeof(int), typeof(SupportBeanComplexProps)};
            IDictionary<String, Object> typeRep = new LinkedHashMap<String, Object>();
            for (int i = 0; i < _testProps.Length; i++) {
                typeRep.Put(_testProps[i], _testTypes[i]);
            }
    
            _testValues = new Object[] {"test", 10, _supportBean};
    
            _eventType = new ObjectArrayEventType(null, "", 1, SupportEventAdapterService.Service, typeRep, null, null, null);
            _eventBean = new ObjectArrayEventBean(_testValues, _eventType);
        }
    
        [Test]
        public void TestGet()
        {
            Assert.AreEqual(_eventType, _eventBean.EventType);
            Assert.AreEqual(_testValues, _eventBean.Underlying);
    
            Assert.AreEqual("test", _eventBean.Get("aString"));
            Assert.AreEqual(10, _eventBean.Get("anInt"));
    
            Assert.AreEqual("NestedValue", _eventBean.Get("myComplexBean.Nested.NestedValue"));
    
            // test wrong property name
            try
            {
                _eventBean.Get("dummy");
                Assert.IsTrue(false);
            }
            catch (PropertyAccessException ex)
            {
                // Expected
                Log.Debug(".testGetter Expected exception, msg=" + ex.Message);
            }
        }
    
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}
