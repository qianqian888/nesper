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
using com.espertech.esper.compat;
using com.espertech.esper.compat.collections;
using com.espertech.esper.support.bean;
using com.espertech.esper.support.epl;
using com.espertech.esper.support.events;
using com.espertech.esper.support.util;
using com.espertech.esper.support.view;

using NUnit.Framework;

namespace com.espertech.esper.view.stat
{
    [TestFixture]
    public class TestUnivariateStatisticsView 
    {
        UnivariateStatisticsView _myView;
        SupportBeanClassView _childView;
    
        [SetUp]
        public void SetUp()
        {
            // Set up sum view and a test child view
            EventType type = UnivariateStatisticsView.CreateEventType(SupportStatementContextFactory.MakeContext(), null, 1);

            UnivariateStatisticsViewFactory factory = new UnivariateStatisticsViewFactory();
            factory.EventType = type;
            factory.FieldExpression = SupportExprNodeFactory.MakeIdentNodeMD("Price");
            _myView = new UnivariateStatisticsView(factory, SupportStatementContextFactory.MakeAgentInstanceViewFactoryContext());
    
            _childView = new SupportBeanClassView(typeof(SupportMarketDataBean));
            _myView.AddView(_childView);
        }
    
        // Check values against Microsoft Excel computed values
        [Test]
        public void TestViewComputedValues()
        {
            // Set up feed for sum view
            SupportStreamImpl stream = new SupportStreamImpl(typeof(SupportMarketDataBean), 3);
            stream.AddView(_myView);
    
            // Send two events to the stream
            Assert.IsTrue(_childView.LastNewData == null);
    
            // Send a first event, checkNew values
            EventBean marketData = MakeBean("IBM", 10, 0);
            stream.Insert(marketData);
            CheckOld(0, 0, Double.NaN, Double.NaN, Double.NaN, Double.NaN);
            CheckNew(1, 10, 10, 0, Double.NaN, Double.NaN);
    
            // Send a second event, checkNew values
            marketData = MakeBean("IBM", 12, 0);
            stream.Insert(marketData);
            CheckOld(1, 10, 10, 0, Double.NaN, Double.NaN);
            CheckNew(2, 22, 11, 1, Math.Sqrt(2.0), 2);
    
            // Send a third event, checkNew values
            marketData = MakeBean("IBM", 9.5, 0);
            stream.Insert(marketData);
            CheckOld(2, 22, 11, 1, Math.Sqrt(2.0), 2);
            CheckNew(3, 31.5, 10.5, 1.08012345, 1.322875656, 1.75);
    
            // Send a 4th event, this time the first event should be gone, checkNew values
            marketData = MakeBean("IBM", 9, 0);
            stream.Insert(marketData);
            CheckOld(3, 31.5, 10.5, 1.08012345, 1.322875656, 1.75);
            CheckNew(3, 30.5, 10.16666667, 1.312334646, 1.607275127, 2.583333333);
        }
    
        [Test]
        public void TestGetSchema()
        {
            Assert.IsTrue(_myView.EventType.GetPropertyType(ViewFieldEnum.UNIVARIATE_STATISTICS__DATAPOINTS.GetName()) == typeof(long?));
            Assert.IsTrue(_myView.EventType.GetPropertyType(ViewFieldEnum.UNIVARIATE_STATISTICS__AVERAGE.GetName()) == typeof(double?));
            Assert.IsTrue(_myView.EventType.GetPropertyType(ViewFieldEnum.UNIVARIATE_STATISTICS__STDDEV.GetName()) == typeof(double?));
            Assert.IsTrue(_myView.EventType.GetPropertyType(ViewFieldEnum.UNIVARIATE_STATISTICS__STDDEVPA.GetName()) == typeof(double?));
            Assert.IsTrue(_myView.EventType.GetPropertyType(ViewFieldEnum.UNIVARIATE_STATISTICS__VARIANCE.GetName()) == typeof(double?));
            Assert.IsTrue(_myView.EventType.GetPropertyType(ViewFieldEnum.UNIVARIATE_STATISTICS__TOTAL.GetName()) == typeof(double?));
        }
    
        [Test]
        public void TestCopyView()
        {
            UnivariateStatisticsView copied = (UnivariateStatisticsView) _myView.CloneView();
            Assert.IsTrue(_myView.FieldExpression.Equals(copied.FieldExpression));
        }
    
        private void CheckNew(long countE, double sumE, double avgE, double stdevpaE, double stdevE, double varianceE)
        {
            IEnumerator<EventBean> iterator = _myView.GetEnumerator();
            CheckValues(iterator.Advance(), countE, sumE, avgE, stdevpaE, stdevE, varianceE);
            Assert.IsTrue(iterator.MoveNext() == false);
    
            Assert.IsTrue(_childView.LastNewData.Length == 1);
            EventBean childViewValues = _childView.LastNewData[0];
            CheckValues(childViewValues, countE, sumE, avgE, stdevpaE, stdevE, varianceE);
        }
    
        private void CheckOld(long countE, double sumE, double avgE, double stdevpaE, double stdevE, double varianceE)
        {
            Assert.IsTrue(_childView.LastOldData.Length == 1);
            EventBean childViewValues = _childView.LastOldData[0];
            CheckValues(childViewValues, countE, sumE, avgE, stdevpaE, stdevE, varianceE);
        }
    
        private void CheckValues(EventBean values, long countE, double sumE, double avgE, double stdevpaE, double stdevE, double varianceE)
        {
            long count = GetLongValue(ViewFieldEnum.UNIVARIATE_STATISTICS__DATAPOINTS, values);
            double sum = GetDoubleValue(ViewFieldEnum.UNIVARIATE_STATISTICS__TOTAL, values);
            double avg = GetDoubleValue(ViewFieldEnum.UNIVARIATE_STATISTICS__AVERAGE, values);
            double stdevpa = GetDoubleValue(ViewFieldEnum.UNIVARIATE_STATISTICS__STDDEVPA, values);
            double stdev = GetDoubleValue(ViewFieldEnum.UNIVARIATE_STATISTICS__STDDEV, values);
            double variance = GetDoubleValue(ViewFieldEnum.UNIVARIATE_STATISTICS__VARIANCE, values);
    
            Assert.AreEqual(count, countE);
            Assert.AreEqual(sum, sumE);
            Assert.IsTrue(DoubleValueAssertionUtil.Equals(avg,  avgE, 6));
            Assert.IsTrue(DoubleValueAssertionUtil.Equals(stdevpa,  stdevpaE, 6));
            Assert.IsTrue(DoubleValueAssertionUtil.Equals(stdev,  stdevE, 6));
            Assert.IsTrue(DoubleValueAssertionUtil.Equals(variance,  varianceE, 6));
        }
    
        private double GetDoubleValue(ViewFieldEnum field, EventBean eventBean)
        {
            return eventBean.Get(field.GetName()).AsDouble();
        }
    
        private long GetLongValue(ViewFieldEnum field, EventBean eventBean)
        {
            return eventBean.Get(field.GetName()).AsLong();
        }
    
        private EventBean MakeBean(String symbol, double price, long volume)
        {
            SupportMarketDataBean bean = new SupportMarketDataBean(symbol, price, volume, "");
            return SupportEventBeanFactory.CreateObject(bean);
        }
    }
}
