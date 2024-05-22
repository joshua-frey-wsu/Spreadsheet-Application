// <copyright file="TextCell.cs" company="PlaceholderCompany">
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
    /// This is a children class that inherits from Cell that handles Text inside of a Cell.
    /// </summary>
    public class TextCell : Cell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextCell"/> class.
        /// </summary>
        /// <param name="rowIndex">The row index of the TextCell.</param>
        /// <param name="columnIndex">The column index of the TextCell.</param>
        public TextCell(int rowIndex, int columnIndex)
            : base(rowIndex, columnIndex)
        {
        }
    }
}
