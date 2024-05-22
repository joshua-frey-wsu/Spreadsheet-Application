// <copyright file="MultiplyNode.cs" company="PlaceholderCompany">
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
    /// This class is the children class of the binaryOperaterNode class and represents the multiplication operation.
    /// </summary>
    public class MultiplyNode : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplyNode"/> class.
        /// </summary>
        public MultiplyNode()
            : base('*')
        {
        }

        /// <summary>
        /// This method evaluates the multiplication of the multiplyNode's two children.
        /// </summary>
        /// <param name="leftValue">The left child value.</param>
        /// <param name="rightValue">The right child value.</param>
        /// <returns>The product.</returns>
        public override double Evaluate(double leftValue, double rightValue)
        {
            return leftValue * rightValue;
        }
    }
}
