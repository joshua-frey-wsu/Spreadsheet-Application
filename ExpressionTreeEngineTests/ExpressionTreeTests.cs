// <copyright file="ExpressionTreeTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Joshua_Frey
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This is a Test Class that test the ExpressionTree class.
    /// </summary>
    [TestClass]
    public class ExpressionTreeTests
    {
        /// <summary>
        /// This Test method tests a addition expression.
        /// </summary>
        [TestMethod]
        public void TestEvaluateAdd()
        {
            ExpressionTree expressionTreeAdd = new ExpressionTree("Add+5+5");
            expressionTreeAdd.SetVariable("Add", 5);
            Assert.AreEqual(15, expressionTreeAdd.Evaluate());
        }

        /// <summary>
        /// This Test method tests a subtraction expression.
        /// </summary>
        [TestMethod]
        public void TestEvaluateSubtract()
        {
            ExpressionTree expressionTreeSubtract = new ExpressionTree("Subtract-5");
            expressionTreeSubtract.SetVariable("Subtract", 10);
            Assert.AreEqual(5, expressionTreeSubtract.Evaluate());
        }

        /// <summary>
        /// This Test method tests a multiplication expression.
        /// </summary>
        [TestMethod]
        public void TestEvaluateMultiply()
        {
            ExpressionTree expressionTreeMultiply = new ExpressionTree("Multiply*5");
            expressionTreeMultiply.SetVariable("Multiply", 5);
            Assert.AreEqual(25, expressionTreeMultiply.Evaluate());
        }

        /// <summary>
        /// This Test method tests a Divide expression.
        /// </summary>
        [TestMethod]
        public void TestEvaluateDivide()
        {
            ExpressionTree expressionTreeDivide = new ExpressionTree("Divide/5");
            expressionTreeDivide.SetVariable("Divide", 25);
            Assert.AreEqual(5, expressionTreeDivide.Evaluate());
        }

        /// <summary>
        /// This Test method tests a Divide by 0 expression.
        /// </summary>
        [TestMethod]
        public void TestEvaluateDivideByZero()
        {
            ExpressionTree expressionTreeDivide = new ExpressionTree("Divide/0");
            expressionTreeDivide.SetVariable("Divide", 5);
            Assert.ThrowsException<DivideByZeroException>(() => expressionTreeDivide.Evaluate());
        }

        /// <summary>
        /// This Test method tests a mixed operation expression.
        /// </summary>
        [TestMethod]
        public void TestEvaluateMixed()
        {
            ExpressionTree expressionTreeMixed = new ExpressionTree("Add+Multiply*Divide/Subtract-5");
            expressionTreeMixed.SetVariable("Add", 5);
            expressionTreeMixed.SetVariable("Multiply", 10);
            expressionTreeMixed.SetVariable("Divide", 2);
            expressionTreeMixed.SetVariable("Subtract", 4);
            Assert.AreEqual(5, expressionTreeMixed.Evaluate());
        }

        /// <summary>
        /// This Test method tests default expressionTree where all variables are defaulted to 0.
        /// </summary>
        [TestMethod]
        public void TestEvaluateDefault()
        {
            ExpressionTree expressionTreeDefault = new ExpressionTree();
            Assert.AreEqual(0, expressionTreeDefault.Evaluate());
        }

        /// <summary>
        /// This Test method tests an expression with parenthesis in it.
        /// </summary>
        [TestMethod]
        public void TestEvaluateParenthesis()
        {
            ExpressionTree expressionTreeParenthesis = new ExpressionTree("(3+2)*(4-1)/5");
            Assert.AreEqual(3, expressionTreeParenthesis.Evaluate());
        }

        /// <summary>
        /// This Test method tests an edge case where the parenthesis are only at the start and beginning of an expression.
        /// This should only evaluate the expresssion on the inside of the parenthesis.
        /// </summary>
        [TestMethod]
        public void TestEvaluateOutsideParenthesis()
        {
            ExpressionTree expressionTreeParenthesis = new ExpressionTree("(2+5-3*2/6)");
            Assert.AreEqual(6, expressionTreeParenthesis.Evaluate());
        }
    }
}
