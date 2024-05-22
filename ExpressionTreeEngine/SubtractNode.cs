// <copyright file="SubtractNode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Joshua_Frey
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class is a children class of the BinaryOperatorNode class and represents the subtraction operation.
    /// </summary>
    public class SubtractNode : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubtractNode"/> class.
        /// </summary>
        public SubtractNode()
            : base('-')
        {
        }

        /// <summary>
        /// This method evaluates the subtraction between the left and right child of the Subtraction Node.
        /// </summary>
        /// <param name="leftValue">The value of the left child.</param>
        /// <param name="rightValue">The value of the right child.</param>
        /// <returns>The difference.</returns>
        public override double Evaluate(double leftValue, double rightValue)
        {
            return leftValue - rightValue;
        }
    }
}
