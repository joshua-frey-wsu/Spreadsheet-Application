// <copyright file="VariableNode.cs" company="PlaceholderCompany">
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
    /// This class represents a Node that takes a variable in a Expression Tree.
    /// </summary>
    public class VariableNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// </summary>
        /// <param name="variable">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        public VariableNode(string variable, double value = 0)
        {
            this.Variable = variable;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the variable preoperty.
        /// </summary>
        public string Variable { get; set; }

        /// <summary>
        /// Gets or sets the value property.
        /// </summary>
        public double Value { get; set; }
    }
}
