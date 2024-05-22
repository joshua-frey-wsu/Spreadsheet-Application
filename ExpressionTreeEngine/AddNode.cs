// <copyright file="AddNode.cs" company="PlaceholderCompany">
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
    /// This is the children class of the BinaryOperatorNode class and represents the addition operation.
    /// </summary>
    public class AddNode : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddNode"/> class.
        /// </summary>
        public AddNode()
            : base('+')
        {
        }

        /// <summary>
        /// This method evaluates the addition of the left child and right child of the addition node.
        /// </summary>
        /// <param name="leftValue">Value of the left child.</param>
        /// <param name="rightValue">Value of the right child.</param>
        /// <returns>The sum.</returns>
        public override double Evaluate(double leftValue, double rightValue)
        {
            return leftValue + rightValue;
        }
    }
}
