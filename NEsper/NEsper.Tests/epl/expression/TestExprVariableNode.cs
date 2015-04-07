///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using com.espertech.esper.epl.expression.core;
using com.espertech.esper.epl.expression.ops;
using com.espertech.esper.epl.variable;
using com.espertech.esper.support.epl;

using NUnit.Framework;

namespace com.espertech.esper.epl.expression
{
    [TestFixture]
    public class TestExprVariableNode 
    {
        private ExprVariableNodeImpl _varNode;
        private VariableService _variableService;
    
        [SetUp]
        public void SetUp()
        {
            _variableService = new VariableServiceImpl(100, null, null, null);
            _variableService.CreateNewVariable(null, "var1", "string", true, false, false, null, null);
            _variableService.CreateNewVariable(null, "dummy", "string", true, false, false, null, null);
            _variableService.CreateNewVariable(null, "IntPrimitive", "int", true, false, false, null, null);
            _varNode = new ExprVariableNodeImpl(_variableService.GetVariableMetaData("var1"), null);
        }
    
        [Test]
        public void TestGetType()  
        {
            SupportExprNodeFactory.Validate3Stream(_varNode);
            Assert.AreEqual(typeof(string), _varNode.ConstantType);
        }
    
        [Test]
        public void TestEvaluate()
        {
            SupportExprNodeFactory.Validate3Stream(_varNode);
            Assert.AreEqual(_varNode.Evaluate(new EvaluateParams(null, true, null)),"my_variable_value");
        }
    
        [Test]
        public void TestEquals()  
        {
            ExprInNode otherInNode = SupportExprNodeFactory.MakeInSetNode(false);
            ExprVariableNode otherVarOne = new ExprVariableNodeImpl(_variableService.GetVariableMetaData("dummy"), null);
            ExprVariableNode otherVarTwo = new ExprVariableNodeImpl(_variableService.GetVariableMetaData("var1"), null);
            ExprVariableNode otherVarThree = new ExprVariableNodeImpl(_variableService.GetVariableMetaData("var1"), "abc");
    
            Assert.IsTrue(_varNode.EqualsNode(_varNode));
            Assert.IsTrue(_varNode.EqualsNode(otherVarTwo));
            Assert.IsFalse(_varNode.EqualsNode(otherVarOne));
            Assert.IsFalse(_varNode.EqualsNode(otherInNode));
            Assert.IsFalse(otherVarTwo.EqualsNode(otherVarThree));
        }
    
        [Test]
        public void TestToExpressionString()
        {
            Assert.AreEqual("var1", _varNode.ToExpressionStringMinPrecedenceSafe());
        }

        private static void TryInvalidValidate(ExprVariableNodeImpl varNode)
        {
            try {
                SupportExprNodeFactory.Validate3Stream(varNode);
                Assert.Fail();
            }
            catch (ExprValidationException ex)
            {
                // expected
            }
        }
    }
}
