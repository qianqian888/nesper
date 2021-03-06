///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

using com.espertech.esper.client;
using com.espertech.esper.compat.collections;
using com.espertech.esper.compat.logging;
using com.espertech.esper.dataflow.annotations;
using com.espertech.esper.dataflow.interfaces;
using com.espertech.esper.dataflow.util;
using com.espertech.esper.epl.expression.core;
using com.espertech.esper.epl.expression;
using com.espertech.esper.events;

namespace com.espertech.esper.dataflow.ops
{
    [DataFlowOperator]
    public class Filter : DataFlowOpLifecycle
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

#pragma warning disable 649
        [DataFlowOpParameter] private ExprNode filter;

        private ExprEvaluator _evaluator;
        private EventBeanSPI _theEvent;
        private readonly EventBean[] _eventsPerStream = new EventBean[1];
        private bool _singleOutputPort;

        [DataFlowContext]
        private EPDataFlowEmitter _graphContext;
#pragma warning restore 649

        public DataFlowOpInitializeResult Initialize(DataFlowOpInitializateContext prepareContext)
        {
            if (prepareContext.InputPorts.Count != 1)
            {
                throw new ExprValidationException("Filter requires single input port");
            }
            if (filter == null)
            {
                throw new ExprValidationException("Required parameter 'filter' providing the filter expression is not provided");
            }
            if ((prepareContext.OutputPorts.Count == 0) || prepareContext.OutputPorts.Count > 2)
            {
                throw new ArgumentException("Filter operator requires one or two output Stream(s) but produces " + prepareContext.OutputPorts.Count + " streams");
            }

            EventType eventType = prepareContext.InputPorts[0].TypeDesc.EventType;
            _singleOutputPort = prepareContext.OutputPorts.Count == 1;

            ExprNode validated = ExprNodeUtility.ValidateSimpleGetSubtree(
                ExprNodeOrigin.DATAFLOWFILTER, filter, prepareContext.StatementContext, eventType, false);
            _evaluator = validated.ExprEvaluator;
            _theEvent = prepareContext.ServicesContext.EventAdapterService.GetShellForType(eventType);
            _eventsPerStream[0] = _theEvent;

            GraphTypeDesc[] typesPerPort = new GraphTypeDesc[prepareContext.OutputPorts.Count];
            for (int i = 0; i < typesPerPort.Length; i++)
            {
                typesPerPort[i] = new GraphTypeDesc(false, true, eventType);
            }
            return new DataFlowOpInitializeResult(typesPerPort);
        }

        public void OnInput(Object row)
        {
            if (Log.IsDebugEnabled)
            {
                Log.Debug("Received row for filtering: " + ((Object[]) row).Render());
            }

            if (!(row is EventBeanSPI))
            {
                _theEvent.Underlying = row;
            }
            else
            {
                _theEvent = (EventBeanSPI)row;
            }

            var pass = (bool?)_evaluator.Evaluate(new EvaluateParams(_eventsPerStream, true, null));
            if (pass ?? false)
            {
                if (Log.IsDebugEnabled)
                {
                    Log.Debug("Submitting row " + ((Object[]) row).Render());
                }

                if (_singleOutputPort)
                {
                    _graphContext.Submit(row);
                }
                else
                {
                    _graphContext.SubmitPort(0, row);
                }
            }
            else
            {
                if (!_singleOutputPort)
                {
                    _graphContext.SubmitPort(1, row);
                }
            }
        }

        public void Open(DataFlowOpOpenContext openContext)
        {
            // no action
        }

        public void Close(DataFlowOpCloseContext openContext)
        {
            // no action
        }
    }
}
