///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using com.espertech.esper.client;
using com.espertech.esper.compat.collections;
using com.espertech.esper.core.service;
using com.espertech.esper.epl.table.mgmt;
using com.espertech.esper.support.bean;
using com.espertech.esper.support.epl;
using com.espertech.esper.support.events;

using NUnit.Framework;

namespace com.espertech.esper.epl.core
{
    [TestFixture]
    public class TestSelectExprJoinWildcardProcessor 
    {
        private SelectExprProcessor _processor;
    
        [SetUp]
        public void SetUp()
        {
            var selectExprEventTypeRegistry = new SelectExprEventTypeRegistry("abc", new StatementEventTypeRefImpl());
            var supportTypes = new SupportStreamTypeSvc3Stream();
    
            _processor = SelectExprJoinWildcardProcessorFactory.Create(Collections.GetEmptyList<int>(), 1, supportTypes.StreamNames, supportTypes.EventTypes,
                    SupportEventAdapterService.Service, null, selectExprEventTypeRegistry, null, null, new Configuration(), new TableServiceImpl());
        }
    
        [Test]
        public void TestProcess()
        {
            EventBean[] testEvents = SupportStreamTypeSvc3Stream.SampleEvents;
    
            EventBean result = _processor.Process(testEvents, true, false, null);
            Assert.AreEqual(testEvents[0].Underlying, result.Get("s0"));
            Assert.AreEqual(testEvents[1].Underlying, result.Get("s1"));
    
            // Test null events, such as in an outer join
            testEvents[1] = null;
            result = _processor.Process(testEvents, true, false, null);
            Assert.AreEqual(testEvents[0].Underlying, result.Get("s0"));
            Assert.IsNull(result.Get("s1"));
        }
    
        [Test]
        public void TestType()
        {
            Assert.AreEqual(typeof(SupportBean), _processor.ResultEventType.GetPropertyType("s0"));
            Assert.AreEqual(typeof(SupportBean), _processor.ResultEventType.GetPropertyType("s1"));
        }
    }
}
