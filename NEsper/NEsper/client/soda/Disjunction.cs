///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;

namespace com.espertech.esper.client.soda
{
	/// <summary>
	/// Disjunction represents a logical OR allowing multiple sub-expressions to be connected by OR.
	/// </summary>
    [Serializable]
    public class Disjunction : Junction
	{
	    /// <summary>
	    /// Ctor - for use to create an expression tree, without child expression.
	    /// <para>
	    /// Use add methods to add child expressions to acts upon.
	    /// </para>
	    /// </summary>
	    public Disjunction()
	    {
	    }

	    /// <summary>Ctor.</summary>
	    /// <param name="first">an expression to add to the OR-test</param>
	    /// <param name="second">an expression to add to the OR-test</param>
	    /// <param name="expressions">is the expression to put in the OR-relationship.</param>
	    public Disjunction(Expression first, Expression second, params Expression[]  expressions)
	    {
	        AddChild(first);
	        AddChild(second);
	        for (int i = 0; i < expressions.Length; i++)
	        {
	            AddChild(expressions[i]);
	        }
	    }

	    public override ExpressionPrecedenceEnum Precedence
	    {
	        get { return ExpressionPrecedenceEnum.OR; }
	    }

        public override void ToPrecedenceFreeEPL(TextWriter writer)
	    {
	        String delimiter = "";
	        foreach (Expression child in this.Children)
	        {
	            writer.Write(delimiter);
	            child.ToEPL(writer, Precedence);
	            delimiter = " or ";
	        }
	    }
	}
} // End of namespace
