///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

using com.espertech.esper.client;
using com.espertech.esper.client.hook;
using com.espertech.esper.compat;
using com.espertech.esper.compat.collections;
using com.espertech.esper.epl.core;
using com.espertech.esper.epl.expression.accessagg;
using com.espertech.esper.epl.expression.core;
using com.espertech.esper.epl.expression;
using com.espertech.esper.epl.expression.methodagg;
using com.espertech.esper.plugin;
using com.espertech.esper.util;

namespace com.espertech.esper.epl.parse
{
    public class ASTAggregationHelper
    {
        public static ExprNode TryResolveAsAggregation(
            EngineImportService engineImportService,
            bool distinct,
            String functionName,
            LazyAllocatedMap<ConfigurationPlugInAggregationMultiFunction, PlugInAggregationMultiFunctionFactory> plugInAggregations,
            String engineURI)
        {
            try
            {
                AggregationFunctionFactory aggregationFactory = engineImportService.ResolveAggregationFactory(functionName);
                return new ExprPlugInAggFunctionFactoryNode(distinct, aggregationFactory, functionName);
            }
            catch (EngineImportUndefinedException)
            {
                // Not an aggregation function
            }
            catch (EngineImportException e)
            {
                throw new IllegalStateException("Error resolving aggregation: " + e.Message, e);
            }

            // try plug-in aggregation multi-function
            ConfigurationPlugInAggregationMultiFunction config =
                engineImportService.ResolveAggregationMultiFunction(functionName);
            if (config != null)
            {
                PlugInAggregationMultiFunctionFactory factory = plugInAggregations.Map.Get(config);
                if (factory == null)
                {
                    factory = TypeHelper.Instantiate<PlugInAggregationMultiFunctionFactory>(
                        config.MultiFunctionFactoryClassName);
                    plugInAggregations.Map.Put(config, factory);
                }
                factory.AddAggregationFunction(
                    new PlugInAggregationMultiFunctionDeclarationContext(
                        functionName.ToLower(), distinct, engineURI, config));
                return new ExprPlugInAggMultiFunctionNode(distinct, config, factory, functionName);
            }

            // try built-in expanded set of aggregation functions
            return engineImportService.ResolveAggExtendedBuiltin(functionName, distinct);
        }
    }
}
