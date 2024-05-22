// <copyright file="NumericalNode.cs" company="PlaceholderCompany">
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
    /// This class represents a Node that takes a constant numerical value as a double in a Expression Tree.
    /// </summary>
    public class NumericalNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericalNode"/> class.
        /// </summary>
        /// <param name="value">The value of the constant numerical node.</param>
        public NumericalNode(double value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets property Value that gets or sets the a constant numerical value.
        /// </summary>
        public double Value { get; set; }
    }
}
