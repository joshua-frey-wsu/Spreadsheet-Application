// <copyright file="ChangeCellBGColorCommand.cs" company="PlaceholderCompany">
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
    /// This is our Command Class for changing the background color of a cell using the ICommand interface.
    /// </summary>
    public class ChangeCellBGColorCommand : ICommand
    {
        private List<(Cell, uint)> oldCellColors;
        private uint newColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeCellBGColorCommand"/> class.
        /// </summary>
        /// <param name="oldCellColors">A list of tuples of all the cells background color being changed and their previous background colors.</param>
        /// <param name="newColor">The new background color of all the cells.</param>
        public ChangeCellBGColorCommand(List<(Cell, uint)> oldCellColors, uint newColor)
        {
            this.oldCellColors = oldCellColors;
            this.newColor = newColor;
        }

        /// <summary>
        /// Implements each action that can be taken with changing the background color of a cell.
        /// </summary>
        public void Execute()
        {
            foreach (var oldCellColor in this.oldCellColors)
            {
                oldCellColor.Item1.BGColor = this.newColor;
            }
        }

        /// <summary>
        /// Implements any action that can be undone with changing the background color of a cell.
        /// </summary>
        public void Undo()
        {
            foreach (var oldCellColor in this.oldCellColors)
            {
                oldCellColor.Item1.BGColor = oldCellColor.Item2;
            }
        }
    }
}
