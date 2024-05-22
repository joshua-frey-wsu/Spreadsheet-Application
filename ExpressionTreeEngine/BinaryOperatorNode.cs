// <copyright file="BinaryOperatorNode.cs" company="PlaceholderCompany">
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
    /// This Class implements a node that stores a binary operator.
    /// </summary>
    public abstract class BinaryOperatorNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperatorNode"/> class.
        /// </summary>
        /// <param name="op">The operation type.</param>
        public BinaryOperatorNode(char op)
        {
            this.Operator = op;
            this.LeftChild = null;
            this.RightChild = null;
        }

        /// <summary>
        /// Gets or sets this is a Operator property.
        /// </summary>
        public char Operator { get; set; }

        /// <summary>
        /// Gets or sets this is the Property for the LeftChild of a binary operator node.
        /// </summary>
        public Node LeftChild { get; set; }

        /// <summary>
        /// Gets or sets this is the Property for the RightChild of a binary operator node.
        /// </summary>
        public Node RightChild { get; set; }

        /// <summary>
        /// Abstract method that performs the operation on the binary operator node's two children.
        /// </summary>
        /// <param name="leftValue">Value of the left child.</param>
        /// <param name="rightValue">Value of the right child.</param>
        /// <returns>Returns the result of the operation.</returns>
        public abstract double Evaluate(double leftValue, double rightValue);
    }
}
