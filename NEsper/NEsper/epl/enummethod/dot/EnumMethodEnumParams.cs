///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using com.espertech.esper.epl.methodbase;

namespace com.espertech.esper.epl.enummethod.dot
{
    public class EnumMethodEnumParams {
    
        public static readonly DotMethodFP[] NOOP_REVERSE = new DotMethodFP[] {
                        new DotMethodFP(DotMethodFPInputEnum.ANY),
                };
    
        public static readonly DotMethodFP[] COUNTOF_FIRST_LAST = new DotMethodFP[] {
                        new DotMethodFP(DotMethodFPInputEnum.ANY),
                        new DotMethodFP(DotMethodFPInputEnum.ANY, new DotMethodFPParam(1, "predicate", DotMethodFPParamTypeEnum.BOOLEAN)),
                };
    
        public static readonly DotMethodFP[] TAKELAST = new DotMethodFP[] {
                        new DotMethodFP(DotMethodFPInputEnum.ANY, new DotMethodFPParam(0, "count", DotMethodFPParamTypeEnum.NUMERIC)),
                };

        public static readonly DotMethodFP[] TAKE = new DotMethodFP[] {
                        new DotMethodFP(DotMethodFPInputEnum.ANY, new DotMethodFPParam(0, "count", DotMethodFPParamTypeEnum.NUMERIC)),
                };
    
        public static readonly DotMethodFP[] AGGREGATE_FP = new DotMethodFP[] {
                        new DotMethodFP(DotMethodFPInputEnum.ANY,
                                new DotMethodFPParam(0, "initialization-value", DotMethodFPParamTypeEnum.ANY),
                                new DotMethodFPParam(2, "(result, next)", DotMethodFPParamTypeEnum.ANY)),
                };
    
        public static readonly DotMethodFP[] ALLOF_ANYOF = new DotMethodFP[] {
                        new DotMethodFP(DotMethodFPInputEnum.ANY, new DotMethodFPParam(1, "predicate", DotMethodFPParamTypeEnum.BOOLEAN)),
                };
    
        public static readonly DotMethodFP[] MIN_MAX = new DotMethodFP[] {
                        new DotMethodFP(DotMethodFPInputEnum.SCALAR_ANY),
                        new DotMethodFP(DotMethodFPInputEnum.SCALAR_ANY, new DotMethodFPParam(1, "value-selector", DotMethodFPParamTypeEnum.ANY)),
                        new DotMethodFP(DotMethodFPInputEnum.EVENTCOLL, new DotMethodFPParam(1, "value-selector", DotMethodFPParamTypeEnum.ANY)),
                };
    
        public static readonly DotMethodFP[] SELECTFROM_MINBY_MAXBY = new DotMethodFP[] {
                        new DotMethodFP(DotMethodFPInputEnum.SCALAR_ANY, new DotMethodFPParam(1, "value-selector", DotMethodFPParamTypeEnum.ANY)),
                        new DotMethodFP(DotMethodFPInputEnum.EVENTCOLL, new DotMethodFPParam(1, "value-selector", DotMethodFPParamTypeEnum.ANY)),
                };
    
        public static readonly DotMethodFP[] AVERAGE_SUMOF = new DotMethodFP[] {
                        new DotMethodFP(DotMethodFPInputEnum.SCALAR_NUMERIC),
                        new DotMethodFP(DotMethodFPInputEnum.SCALAR_ANY, new DotMethodFPParam(1, "value-selector", DotMethodFPParamTypeEnum.NUMERIC)),
                        new DotMethodFP(DotMethodFPInputEnum.EVENTCOLL, new DotMethodFPParam(1, "value-selector", DotMethodFPParamTypeEnum.NUMERIC))
                };
    
        public static readonly DotMethodFP[] MOST_LEAST_FREQ = new DotMethodFP[] {
                        new DotMethodFP(DotMethodFPInputEnum.SCALAR_ANY),
                        new DotMethodFP(DotMethodFPInputEnum.SCALAR_ANY, new DotMethodFPParam(1, "value-selector", DotMethodFPParamTypeEnum.ANY)),
                        new DotMethodFP(DotMethodFPInputEnum.EVENTCOLL, new DotMethodFPParam(1, "value-selector", DotMethodFPParamTypeEnum.ANY))
                };
    
        public static readonly DotMethodFP[] MAP = new DotMethodFP[] {
                        new DotMethodFP(DotMethodFPInputEnum.SCALAR_ANY,
                                new DotMethodFPParam(1, "key-selector", DotMethodFPParamTypeEnum.ANY),
                                new DotMethodFPParam(1, "value-selector", DotMethodFPParamTypeEnum.ANY)),
                        new DotMethodFP(DotMethodFPInputEnum.EVENTCOLL,
                                new DotMethodFPParam(1, "key-selector", DotMethodFPParamTypeEnum.ANY),
                                new DotMethodFPParam(1, "value-selector", DotMethodFPParamTypeEnum.ANY)),
                };
    
        public static readonly DotMethodFP[] GROUP = new DotMethodFP[] {
                        new DotMethodFP(DotMethodFPInputEnum.SCALAR_ANY, new DotMethodFPParam(1, "key-selector", DotMethodFPParamTypeEnum.ANY)),
                        new DotMethodFP(DotMethodFPInputEnum.EVENTCOLL, new DotMethodFPParam(1, "key-selector", DotMethodFPParamTypeEnum.ANY)),
                        new DotMethodFP(DotMethodFPInputEnum.SCALAR_ANY,
                                new DotMethodFPParam(1, "key-selector", DotMethodFPParamTypeEnum.ANY),
                                new DotMethodFPParam(1, "value-selector", DotMethodFPParamTypeEnum.ANY)),
                        new DotMethodFP(DotMethodFPInputEnum.EVENTCOLL,
                                new DotMethodFPParam(1, "key-selector", DotMethodFPParamTypeEnum.ANY),
                                new DotMethodFPParam(1, "value-selector", DotMethodFPParamTypeEnum.ANY)),
                };
    
        public static readonly DotMethodFP[] ORDERBY_DISTINCT = new DotMethodFP[] {
                        new DotMethodFP(DotMethodFPInputEnum.SCALAR_ANY),
                        new DotMethodFP(DotMethodFPInputEnum.SCALAR_ANY, new DotMethodFPParam(1, "compare-selector", DotMethodFPParamTypeEnum.ANY)),
                        new DotMethodFP(DotMethodFPInputEnum.EVENTCOLL, new DotMethodFPParam(1, "compare-selector", DotMethodFPParamTypeEnum.ANY)),
                };
    
        public static readonly DotMethodFP[] WHERE_FP = new DotMethodFP[] {
                        new DotMethodFP(DotMethodFPInputEnum.ANY, new DotMethodFPParam(1, "predicate", DotMethodFPParamTypeEnum.BOOLEAN)),
                        new DotMethodFP(DotMethodFPInputEnum.ANY, new DotMethodFPParam(2, "(predicate, index)", DotMethodFPParamTypeEnum.BOOLEAN))
                };
    
        public static readonly DotMethodFP[] SET_LOGIC_FP = new DotMethodFP[] {
                        new DotMethodFP(DotMethodFPInputEnum.ANY, new DotMethodFPParam(0, "collection", DotMethodFPParamTypeEnum.ANY)),
                };
    
        public static readonly DotMethodFP[] SEQ_EQUALS_FP = new DotMethodFP[] {
                        new DotMethodFP(DotMethodFPInputEnum.SCALAR_ANY, new DotMethodFPParam(0, "sequence", DotMethodFPParamTypeEnum.ANY)),
                };
    }
}
