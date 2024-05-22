// <copyright file="DivideNode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Joshua_Frey
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class is a children class of the BinaryOperatorNode Class and represents the dividing operation.
    /// </summary>
    public class DivideNode : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DivideNode"/> class.
        /// </summary>
        public DivideNode()
            : base('/')
        {
        }

        /// <summary>
        /// This method evaluates a divsion of two numbers.
        /// </summary>
        /// <param name="leftValue">The value of the left child.</param>
        /// <param name="rightValue">The value of the right child.</param>
        /// <returns>The quotient.</returns>
        /// <exception cref="DivideByZeroException">Throws an exception if divided by zero.</exception>
        public override double Evaluate(double leftValue, double rightValue)
        {
            if (rightValue != 0)
            {
                return leftValue / rightValue;
            }

            throw new DivideByZeroException("Cannot Divide By Zero");
        }
    }
}
