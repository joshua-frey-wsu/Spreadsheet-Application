// <copyright file="OperatorNodeFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Joshua_Frey
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This is a Factory Class that will creating the proper children operator node for a Expression Tree.
    /// </summary>
    public static class OperatorNodeFactory
    {
        private static Dictionary<char, Type> operators = new Dictionary<char, Type>
        {
            { '+', typeof(AddNode) },
            { '-', typeof(SubtractNode) },
            { '*', typeof(MultiplyNode) },
            { '/', typeof(DivideNode) },
        };

        /// <summary>
        /// Logic for the creation of operator nodes with the propery opeartor type and children operator node using reflection to dynamically populate handled operators.
        /// </summary>
        /// <param name="op">The opeartion type.</param>
        /// <returns>A BinaryOperatorNode.</returns>
        public static BinaryOperatorNode CreateOperatorNode(char op)
        {
            if (operators.ContainsKey(op))
            {
                object operatorNodeObject = System.Activator.CreateInstance(operators[op]);
                if (operatorNodeObject is BinaryOperatorNode)
                {
                    return (BinaryOperatorNode)operatorNodeObject;
                }
            }

            throw new Exception("Unhandled operator");
        }
    }
}