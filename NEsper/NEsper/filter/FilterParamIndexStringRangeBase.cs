///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

using com.espertech.esper.compat.collections;
using com.espertech.esper.compat.threading;

namespace com.espertech.esper.filter
{
    public abstract class FilterParamIndexStringRangeBase : FilterParamIndexLookupableBase
    {
        protected readonly OrderedDictionary<StringRange, EventEvaluator> Ranges;
        private readonly IDictionary<StringRange, EventEvaluator> _rangesNullEndpoints;
        private readonly IReaderWriterLock _rangesRwLock;

        protected FilterParamIndexStringRangeBase(FilterSpecLookupable lookupable, IReaderWriterLock readWriteLock, FilterOperator filterOperator)
            : base(filterOperator, lookupable)
        {
            Ranges = new OrderedDictionary<StringRange, EventEvaluator>(new StringRangeComparator());
            _rangesNullEndpoints = new Dictionary<StringRange, EventEvaluator>();
            _rangesRwLock = readWriteLock;
        }

        protected internal override EventEvaluator Get(Object expressionValue)
        {
            if (!(expressionValue is StringRange))
            {
                throw new ArgumentException("Supplied expressionValue must be of type StringRange");
            }

            var range = (StringRange)expressionValue;
            if ((range.Max == null) || (range.Min == null))
            {
                return _rangesNullEndpoints.Get(range);
            }

            return Ranges.Get(range);
        }

        protected internal override void Put(Object expressionValue, EventEvaluator matcher)
        {
            if (!(expressionValue is StringRange))
            {
                throw new ArgumentException("Supplied expressionValue must be of type DoubleRange");
            }

            var range = (StringRange)expressionValue;
            if ((range.Max == null) || (range.Min == null))
            {
                _rangesNullEndpoints.Put(range, matcher);     // endpoints null - we don't enter
                return;
            }

            Ranges.Put(range, matcher);
        }

        public override bool Remove(Object filterConstant)
        {
            var range = (StringRange)filterConstant;
            if ((range.Max == null) || (range.Min == null))
            {
                return _rangesNullEndpoints.Pluck(range) != null;
            }

            return Ranges.Pluck(range) != null;
        }

        public override int Count
        {
            get { return Ranges.Count; }
        }

        public override IReaderWriterLock ReadWriteLock
        {
            get { return _rangesRwLock; }
        }
    }
}
