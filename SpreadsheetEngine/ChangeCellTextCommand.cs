// <copyright file="ChangeCellTextCommand.cs" company="PlaceholderCompany">
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
    /// This is our Command Class for changing the text of a cell using the ICommand interface.
    /// </summary>
    public class ChangeCellTextCommand : ICommand
    {
        private readonly Cell cell;
        private readonly string oldText;
        private readonly string newText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeCellTextCommand"/> class.
        /// </summary>
        /// <param name="cell">The cell being changed.</param>
        /// <param name="oldText">The old text of the cell.</param>
        /// <param name="newText">The new text of the cell.</param>
        public ChangeCellTextCommand(Cell cell, string oldText, string newText)
        {
            this.cell = cell;
            this.oldText = oldText;
            this.newText = newText;
        }

        /// <summary>
        /// Implements each action that can be taken with changing the text of a cell.
        /// </summary>
        public void Execute()
        {
            // Execute the command (set new text)
            this.cell.Text = this.newText;
        }

        /// <summary>
        /// Implements any action that can be undone with changing the text of a cell.
        /// </summary>
        public void Undo()
        {
            this.cell.Text = this.oldText;
        }
    }
}
