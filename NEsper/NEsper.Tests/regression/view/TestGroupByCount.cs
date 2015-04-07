///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

using com.espertech.esper.client;
using com.espertech.esper.client.scopetest;
using com.espertech.esper.client.soda;
using com.espertech.esper.metrics.instrumentation;
using com.espertech.esper.support.bean;
using com.espertech.esper.support.client;
using com.espertech.esper.util;

using com.espertech.esper.compat.logging;

using NUnit.Framework;

namespace com.espertech.esper.regression.view
{
    [TestFixture]
    public class TestGroupByCount 
    {
        private const String SYMBOL_DELL = "DELL";
        private const String SYMBOL_IBM = "IBM";
    
        private EPServiceProvider _epService;
        private SupportUpdateListener _testListener;
        private EPStatement _selectTestView;
    
        [SetUp]
        public void SetUp()
        {
            _testListener = new SupportUpdateListener();
            _epService = EPServiceProviderManager.GetDefaultProvider(SupportConfigFactory.GetConfiguration());
            _epService.Initialize();
            if (InstrumentationHelper.ENABLED) { InstrumentationHelper.StartTest(_epService, GetType(), GetType().FullName); }
        }

        [TearDown]
        public void TearDown()
        {
            if (InstrumentationHelper.ENABLED) { InstrumentationHelper.EndTest(); }
            _testListener = null;
        }
    
        [Test]
        public void TestCountOneViewOM()
        {
            EPStatementObjectModel model = new EPStatementObjectModel();
            model.SelectClause = SelectClause.Create().SetStreamSelector(StreamSelector.RSTREAM_ISTREAM_BOTH)
                .Add("Symbol")
                .Add(Expressions.CountStar(), "countAll")
                .Add(Expressions.CountDistinct("Volume"), "countDistVol")
                .Add(Expressions.Count("Volume"), "countVol");
            model.FromClause = FromClause.Create(FilterStream.Create(typeof(SupportMarketDataBean).FullName).AddView("win", "length", Expressions.Constant(3)));
            model.WhereClause = Expressions.Or()
                .Add(Expressions.Eq("Symbol", "DELL"))
                .Add(Expressions.Eq("Symbol", "IBM"))
                .Add(Expressions.Eq("Symbol", "GE"));
            model.GroupByClause = GroupByClause.Create("Symbol");
            model = (EPStatementObjectModel) SerializableObjectCopier.Copy(model);
    
            String viewExpr = "select irstream Symbol, " +
                                      "count(*) as countAll, " +
                                      "count(distinct Volume) as countDistVol, " +
                                      "count(Volume) as countVol" +
                              " from " + typeof(SupportMarketDataBean).FullName + ".win:length(3) " +
                              "where Symbol=\"DELL\" or Symbol=\"IBM\" or Symbol=\"GE\" " +
                              "group by Symbol";
            Assert.AreEqual(viewExpr, model.ToEPL());
    
            _selectTestView = _epService.EPAdministrator.Create(model);
            _selectTestView.Events += _testListener.Update;
    
            RunAssertion();
        }
    
        [Test]
        public void TestGroupByCountNestedAggregationAvg()
        {
            // test for ESPER-328
            String viewExpr = "select Symbol, count(*) as cnt, avg(count(*)) as val from " + typeof(SupportMarketDataBean).FullName + ".win:length(3)" +
                              "group by Symbol order by Symbol asc";
            EPStatement stmt = _epService.EPAdministrator.CreateEPL(viewExpr);
            stmt.Events += _testListener.Update;
    
            SendEvent(SYMBOL_DELL, 50L);
            EPAssertionUtil.AssertProps(_testListener.AssertOneGetNewAndReset(), "Symbol,cnt,val".Split(','), new Object[] {"DELL", 1L, 1d});
    
            SendEvent(SYMBOL_DELL, 51L);
            EPAssertionUtil.AssertProps(_testListener.AssertOneGetNewAndReset(), "Symbol,cnt,val".Split(','), new Object[] {"DELL", 2L, 1.5d});
    
            SendEvent(SYMBOL_DELL, 52L);
            EPAssertionUtil.AssertProps(_testListener.AssertOneGetNewAndReset(), "Symbol,cnt,val".Split(','), new Object[] {"DELL", 3L, 2d});
    
            SendEvent("IBM", 52L);
            EventBean[] events = _testListener.LastNewData;
            EPAssertionUtil.AssertProps(events[0], "Symbol,cnt,val".Split(','), new Object[] {"DELL", 2L, 2d});
            EPAssertionUtil.AssertProps(events[1], "Symbol,cnt,val".Split(','), new Object[] {"IBM", 1L, 1d});
            _testListener.Reset();
    
            SendEvent(SYMBOL_DELL, 53L);
            EPAssertionUtil.AssertProps(_testListener.AssertOneGetNewAndReset(), "Symbol,cnt,val".Split(','), new Object[] {"DELL", 2L, 2.5d});
        }
    
        [Test]
        public void TestCountOneViewCompile()
        {
            String viewExpr = "select irstream Symbol, " +
                                      "count(*) as countAll, " +
                                      "count(distinct Volume) as countDistVol, " +
                                      "count(Volume) as countVol" +
                              " from " + typeof(SupportMarketDataBean).FullName + ".win:length(3) " +
                              "where Symbol=\"DELL\" or Symbol=\"IBM\" or Symbol=\"GE\" " +
                              "group by Symbol";
            EPStatementObjectModel model = _epService.EPAdministrator.CompileEPL(viewExpr);
            model = (EPStatementObjectModel) SerializableObjectCopier.Copy(model);
            Assert.AreEqual(viewExpr, model.ToEPL());
    
            _selectTestView = _epService.EPAdministrator.Create(model);
            _selectTestView.Events += _testListener.Update;
    
            RunAssertion();
        }
    
        [Test]
        public void TestCountOneView()
        {
            String viewExpr = "select irstream Symbol, " +
                                      "count(*) as countAll," +
                                      "count(distinct Volume) as countDistVol," +
                                      "count(all Volume) as countVol" +
                              " from " + typeof(SupportMarketDataBean).FullName + ".win:length(3) " +
                              "where Symbol='DELL' or Symbol='IBM' or Symbol='GE' " +
                              "group by Symbol";
    
            _selectTestView = _epService.EPAdministrator.CreateEPL(viewExpr);
            _selectTestView.Events += _testListener.Update;
    
            RunAssertion();
        }
    
        [Test]
        public void TestCountJoin()
        {
            String viewExpr = "select irstream Symbol, " +
                                      "count(*) as countAll," +
                                      "count(distinct Volume) as countDistVol," +
                                      "count(Volume) as countVol " +
                              " from " + typeof(SupportBeanString).FullName + ".win:length(100) as one, " +
                                        typeof(SupportMarketDataBean).FullName + ".win:length(3) as two " +
                              "where (Symbol='DELL' or Symbol='IBM' or Symbol='GE') " +
                              "  and one.TheString = two.Symbol " +
                              "group by Symbol";
    
            _selectTestView = _epService.EPAdministrator.CreateEPL(viewExpr);
            _selectTestView.Events += _testListener.Update;
    
            _epService.EPRuntime.SendEvent(new SupportBeanString(SYMBOL_DELL));
            _epService.EPRuntime.SendEvent(new SupportBeanString(SYMBOL_IBM));
    
            RunAssertion();
        }
    
        private void RunAssertion()
        {
            // assert select result type
            Assert.AreEqual(typeof(string), _selectTestView.EventType.GetPropertyType("Symbol"));
            Assert.AreEqual(typeof(long?), _selectTestView.EventType.GetPropertyType("countAll"));
            Assert.AreEqual(typeof(long?), _selectTestView.EventType.GetPropertyType("countDistVol"));
            Assert.AreEqual(typeof(long?), _selectTestView.EventType.GetPropertyType("countVol"));
    
            SendEvent(SYMBOL_DELL, 50L);
            AssertEvents(SYMBOL_DELL, 0L, 0L, 0L,
                    SYMBOL_DELL, 1L, 1L, 1L
                    );
    
            SendEvent(SYMBOL_DELL, null);
            AssertEvents(SYMBOL_DELL, 1L, 1L, 1L,
                    SYMBOL_DELL, 2L, 1L, 1L
                    );
    
            SendEvent(SYMBOL_DELL, 25L);
            AssertEvents(SYMBOL_DELL, 2L, 1L, 1L,
                    SYMBOL_DELL, 3L, 2L, 2L
                    );
    
            SendEvent(SYMBOL_DELL, 25L);
            AssertEvents(SYMBOL_DELL, 3L, 2L, 2L,
                    SYMBOL_DELL, 3L, 1L, 2L
                    );
    
            SendEvent(SYMBOL_DELL, 25L);
            AssertEvents(SYMBOL_DELL, 3L, 1L, 2L,
                    SYMBOL_DELL, 3L, 1L, 3L
                    );
    
            SendEvent(SYMBOL_IBM, 1L);
            SendEvent(SYMBOL_IBM, null);
            SendEvent(SYMBOL_IBM, null);
            SendEvent(SYMBOL_IBM, null);
            AssertEvents(SYMBOL_IBM, 3L, 1L, 1L,
                    SYMBOL_IBM, 3L, 0L, 0L
                    );
        }

        private void AssertEvents(String symbolOld, long? countAllOld, long? countDistVolOld, long? countVolOld,
                                  String symbolNew, long? countAllNew, long? countDistVolNew, long? countVolNew)
        {
            EventBean[] oldData = _testListener.LastOldData;
            EventBean[] newData = _testListener.LastNewData;
    
            Assert.AreEqual(1, oldData.Length);
            Assert.AreEqual(1, newData.Length);
    
            Assert.AreEqual(symbolOld, oldData[0].Get("Symbol"));
            Assert.AreEqual(countAllOld, oldData[0].Get("countAll"));
            Assert.AreEqual(countDistVolOld, oldData[0].Get("countDistVol"));
            Assert.AreEqual(countVolOld, oldData[0].Get("countVol"));
    
            Assert.AreEqual(symbolNew, newData[0].Get("Symbol"));
            Assert.AreEqual(countAllNew, newData[0].Get("countAll"));
            Assert.AreEqual(countDistVolNew, newData[0].Get("countDistVol"));
            Assert.AreEqual(countVolNew, newData[0].Get("countVol"));
    
            _testListener.Reset();
            Assert.IsFalse(_testListener.IsInvoked);
        }

        private void SendEvent(String symbol, long? volume)
        {
            SupportMarketDataBean bean = new SupportMarketDataBean(symbol, 0, volume, null);
            _epService.EPRuntime.SendEvent(bean);
        }
    
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}
