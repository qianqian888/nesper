///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

using com.espertech.esper.client;
using com.espertech.esper.client.scopetest;
using com.espertech.esper.client.time;
using com.espertech.esper.compat;
using com.espertech.esper.compat.collections;
using com.espertech.esper.metrics.instrumentation;
using com.espertech.esper.support.bean;
using com.espertech.esper.support.client;

using NUnit.Framework;

namespace com.espertech.esper.regression.nwtable
{
    [TestFixture]
    public class TestNamedWindowOutputrate 
    {
        private EPServiceProvider epService;
        private SupportUpdateListener listener;
    
        [SetUp]
        public void SetUp()
        {
            Configuration config = SupportConfigFactory.GetConfiguration();
            epService = EPServiceProviderManager.GetDefaultProvider(config);
            epService.Initialize();
            if (InstrumentationHelper.ENABLED) { InstrumentationHelper.StartTest(epService, this.GetType(), GetType().FullName);}
            listener = new SupportUpdateListener();
        }
    
        [TearDown]
        public void TearDown() {
            if (InstrumentationHelper.ENABLED) { InstrumentationHelper.EndTest();}
            listener = null;
        }
    
        [Test]
        public void TestOutputSnapshot() {
            epService.EPAdministrator.CreateEPL("create schema SupportBean as " + typeof(SupportBean).FullName);
    
            epService.EPAdministrator.CreateEPL("create window MyWindowOne.win:keepall() as (TheString string, intv int)");
            epService.EPAdministrator.CreateEPL("insert into MyWindowOne select TheString, IntPrimitive as intv from SupportBean");
    
            epService.EPRuntime.SendEvent(new CurrentTimeEvent(0));
    
            string[] fields = new string[] {"TheString","c"};
            EPStatement stmtSelect = epService.EPAdministrator.CreateEPL("select irstream TheString, count(*) as c from MyWindowOne group by TheString output snapshot every 1 second");
            stmtSelect.AddListener(listener);
    
            epService.EPRuntime.SendEvent(new SupportBean("A", 1));
            epService.EPRuntime.SendEvent(new SupportBean("A", 2));
            epService.EPRuntime.SendEvent(new SupportBean("B", 4));
    
            epService.EPRuntime.SendEvent(new CurrentTimeEvent(1000));

            EPAssertionUtil.AssertPropsPerRow(listener.GetAndResetLastNewData(), fields, new object[][] { new object[] { "A", 2L }, new object[] { "B", 1L } });
    
            epService.EPRuntime.SendEvent(new SupportBean("B", 5));
            epService.EPRuntime.SendEvent(new CurrentTimeEvent(2000));

            EPAssertionUtil.AssertPropsPerRow(listener.GetAndResetLastNewData(), fields, new object[][] { new object[] { "A", 2L }, new object[] { "B", 2L } });
    
            epService.EPRuntime.SendEvent(new CurrentTimeEvent(3000));

            EPAssertionUtil.AssertPropsPerRow(listener.GetAndResetLastNewData(), fields, new object[][] { new object[] { "A", 2L }, new object[] { "B", 2L } });
    
            epService.EPRuntime.SendEvent(new SupportBean("A", 5));
            epService.EPRuntime.SendEvent(new SupportBean("C", 1));
            epService.EPRuntime.SendEvent(new CurrentTimeEvent(4000));

            EPAssertionUtil.AssertPropsPerRow(listener.GetAndResetLastNewData(), fields, new object[][] { new object[] { "A", 3L }, new object[] { "B", 2L }, new object[] { "C", 1L } });
        }
    }
}
