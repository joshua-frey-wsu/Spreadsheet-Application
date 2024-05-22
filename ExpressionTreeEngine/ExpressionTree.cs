// <copyright file="ExpressionTree.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Joshua_Frey
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents the implementation of an Expression Tree.
    /// </summary>
    public class ExpressionTree
    {
        private string expression;
        private string defaultExpression = "A1+B1+C1";
        private Node root;
        private Dictionary<string, double> variables = new Dictionary<string, double>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// This is a constructor that takes an expression as a parameter.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public ExpressionTree(string expression)
        {
            this.expression = expression;
            this.root = this.CreateExpTree(expression, 0, expression.Length - 1); // creates the expression tree starting from the root.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// This is a constructor that doesn't take any expressions as a parameter so it uses the default expression.
        /// </summary>
        public ExpressionTree()
        {
            this.expression = this.defaultExpression; // sets it to the default expression if no expression is passed in.
            this.root = this.CreateExpTree(this.defaultExpression, 0, this.defaultExpression.Length - 1); // creates the expression tree.
        }

        /// <summary>
        /// Gets or sets this is an Expression Property.
        /// </summary>
        public string Expression
        {
            get
            {
                return this.expression;
            }

            set
            {
                this.expression = value;
                this.variables.Clear(); // Everytime a new expression is set clear the variables out of the variables dictionary so you can make new variables for that expression
            }
        }

        /// <summary>
        /// This method sets the variable to a numeric value and stores it in a dictionary so the same variable can't be made again.
        /// </summary>
        /// <returns>Returns a list of the variable names in the expression tree.</returns>
        public List<string> GetVariableNames()
        {
            List<string> variableNames = new List<string>();
            foreach (string varName in this.variables.Keys)
            {
                variableNames.Add(varName);
            }

            return variableNames;
        }

        /// <summary>
        /// This method sets the variable to a numeric value and stores it in a dictionary so the same variable can't be made again.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="variableValue">Value of the variable.</param>
        public void SetVariable(string variableName, double variableValue)
        {
           this.variables[variableName] = variableValue;
        }

        /// <summary>
        /// This method evaluates the expression tree.
        /// </summary>
        /// <returns>The result of the expression.</returns>
        public double Evaluate()
        {
            return this.EvaluateHelper(this.root);
        }

        private Node CreateExpTree(string expression, int startingExpIndex, int lastExpIndex)
        {
            // Dictionary that stores the precedence of an operation.
            // Multipication and Division have a higher precedence than addition and subtraction.
            Dictionary<char, int> precedence = new Dictionary<char, int>
            {
                { '*', 2 },
                { '/', 2 },
                { '+', 1 },
                { '-', 1 },
            };

            // Initializes the parenthesis counter, index of the operation, and the precedence of the current operation.
            int parenthesisCounter = 0;
            int opIndex = -1;
            int opPrecedence = int.MaxValue;

            // Starts from the right to the left to get left to right precedence while also checking for operations.
            for (int i = lastExpIndex; i >= startingExpIndex; i--)
            {
                // The current character in the expression string.
                char character = expression[i];

                // If there is a open parenthesis then increment the parenthesis counter.
                if (character == '(')
                {
                    parenthesisCounter++;
                }
                else if (character == ')')
                {
                    // If there is a close parenthesis then decrement the parenthesis counter.
                    parenthesisCounter--;
                }

                // if the parenthesis counter is 0 and the opeartion is in the precedence dictionary and the precedence of the current operation is less than or equal to the last operation.
                // Then set the new operation index to the current operation's index and the new operation precedence to the current operation's precedence.
                if (parenthesisCounter == 0 && precedence.ContainsKey(character) && precedence[character] < opPrecedence)
                {
                    opIndex = i;
                    opPrecedence = precedence[character];
                }
            }

            // If the parenthesis counter is 0 and the operationIndex isn't -1 then an operation was found in the expression since the opIndex got set to a different value.
            if (parenthesisCounter == 0 && opIndex != -1)
            {
                // An operator outside of parentheses was found
                // Creates the propery BinaryOperatorNode type using a Factory Class.
                BinaryOperatorNode binaryOperatorNode = OperatorNodeFactory.CreateOperatorNode(expression[opIndex]);

                // Sets the left child to the left side of the expression from the current position of the opeartion in the expression.
                // Sets the right child to the right side of the expresssion from the current position of the operation in the expression.
                binaryOperatorNode.LeftChild = this.CreateExpTree(expression, startingExpIndex, opIndex - 1);
                binaryOperatorNode.RightChild = this.CreateExpTree(expression, opIndex + 1, lastExpIndex);
                return binaryOperatorNode;
            }
            else if (expression[startingExpIndex] == '(' && expression[lastExpIndex] == ')')
            {
                // If the expression starts and ends with parenthesis then evaluate the expresssion inside of those parenthesis.
                return this.CreateExpTree(expression, startingExpIndex + 1, lastExpIndex - 1);
            }
            else
            {
                double num = 0;

                // Parses the substring of the expression to find if a number or a variable is in the expression and returns the proper type of node accordingly.
                if (double.TryParse(expression.Substring(startingExpIndex, lastExpIndex - startingExpIndex + 1), out num))
                {
                    return new NumericalNode(num)
                    {
                        Value = num,
                    };
                }
                else
                {
                    this.variables[expression.Substring(startingExpIndex, lastExpIndex - startingExpIndex + 1)] = 0; // Sets the default value for the variable to 0.
                    return new VariableNode(expression.Substring(startingExpIndex, lastExpIndex - startingExpIndex + 1))
                    {
                        Variable = expression.Substring(startingExpIndex, lastExpIndex - startingExpIndex + 1),
                    };
                }
            }
        }

        private double EvaluateHelper(Node node)
        {
            // If the node is null throw a ArgumentNullException
            if (node == null)
            {
                throw new ArgumentNullException("Node is Null");
            }

            // If the node is a NumericalNode return its value to be evaluated
            if (node is NumericalNode)
            {
                NumericalNode numericalNode = (NumericalNode)node;
                return numericalNode.Value;
            }

            // If the node is a VariableNode then find the variable in the dictionary where the value for the variable is set and return that value if found
            if (node is VariableNode)
            {
                VariableNode variableNode = (VariableNode)node;

                // if the variable is found in the variables dictionary the return its value
                if (this.variables.ContainsKey(variableNode.Variable))
                {
                    return this.variables[variableNode.Variable];
                }

                // Else throw an ArgumentException
                throw new ArgumentException("Variable " + variableNode.Variable + " Not Found");
            }

            // If the node is a BinaryOperaterNode then perform its operation on its LeftChild and RightChild
            if (node is BinaryOperatorNode)
            {
                BinaryOperatorNode binaryOperatorNode = (BinaryOperatorNode)node;

                return binaryOperatorNode.Evaluate(this.EvaluateHelper(binaryOperatorNode.LeftChild), this.EvaluateHelper(binaryOperatorNode.RightChild));
            }

            throw new ArgumentException("Node is Not Properly Created in Expression Tree");
        }
    }
}
