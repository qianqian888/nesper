﻿///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;

using com.espertech.esper.compat.collections;
using com.espertech.esper.epl.spec;
using com.espertech.esper.script;
using com.espertech.esper.util;

using Noesis.Javascript;

namespace NEsper.Scripting.Noesis
{
    public class ScriptingEngineNoesis : ScriptingEngine
    {
        public ScriptingEngineNoesis()
        {
            Language = "javascript";
            LanguagePrefix = "js";
        }

        public string Language { get; private set; }

        public string LanguagePrefix { get; private set; }

        public Func<ScriptArgs, Object> Compile(ExpressionScriptProvided expressionScript)
        {
            return args => ExecuteWithScriptArgs(expressionScript, args);
        }

        private object ExecuteWithScriptArgs(ExpressionScriptProvided expressionScript, ScriptArgs args)
        {
            using (var context = new JavascriptContext())
            {
                foreach (var binding in args.Bindings)
                {
                    context.SetParameter(binding.Key, binding.Value);
                }

                context.SetParameter("clr", new ClrBinding());
                context.SetParameter("print", new Action<object>(value => Console.Out.WriteLine(value)));
                context.SetParameter("render", new Action<object>(value => value.Render()));
                return context.Run(expressionScript.Expression);
            }
        }

        public void Verify(ExpressionScriptProvided script)
        {
        }

        public class TypeWrapper    
        {
            public Type Type { get; set; }

            public TypeWrapper(Type type)
            {
                Type = type;
            }

            public object New(object[] args)
            {
                return ConstructorHelper.InvokeConstructor(Type, args);
            }
        }

        public class ClrBinding
        {
            public TextReader Stdin
            {
                get { return Console.In; }
            }

            public TextWriter Stdout
            {
                get { return Console.Out; }
            }

            public TextWriter Stderr
            {
                get { return Console.Error; }
            }

            public AppDomain CurrentAppDomain
            {
                get { return AppDomain.CurrentDomain; }
            }

            public object New(string typeName, object[] args)
            {
                var type = TypeHelper.ResolveType(typeName, true);
                return ConstructorHelper.InvokeConstructor(type, args);
            }

            public object ImportClass(string typeName)
            {
                var type = TypeHelper.ResolveType(typeName, true);
                return new TypeWrapper(type);
            }
        }
    }
}
