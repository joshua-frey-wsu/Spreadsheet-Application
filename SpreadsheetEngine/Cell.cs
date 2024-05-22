// <copyright file="Cell.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Joshua_Frey
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    /// <summary>
    /// This class implements the logic of each Cell in the SpreadSheet.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        /// <summary>
        /// string that contains text in the cell.
        /// </summary>
        private string text;

        /// <summary>
        /// string that contains the value of the evaulation of a formula.
        /// </summary>
        private string value;

        /// <summary>
        /// uint that contains the background color of a cell.
        /// </summary>
        private uint bgColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="rowIndex">This is the row index of the Cell.</param>
        /// <param name="columnIndex">This is the column index of the Cell.</param>
        public Cell(int rowIndex, int columnIndex)
        {
            this.RowIndex = rowIndex;
            this.ColumnIndex = columnIndex;
            this.BGColor = 0xFFFFFFFF; // sets the default background color to white.
            this.Text = string.Empty; // sets the text initally as an empty string.
            this.Value = string.Empty; // sets the value initially as an empty string
            this.SubscribedCells = new List<Cell>(); // initializes to an empty list of cells.
        }

        /// <summary>
        /// This is an event handler for when a property of a cell changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Gets or sets a list of subscribed cells to the current cell.
        /// </summary>
        public List<Cell> SubscribedCells { get; set; }

        /// <summary>
        /// Gets row index of the cell.
        /// </summary>
        public int RowIndex { get; }

        /// <summary>
        /// Gets column index of the cell.
        /// </summary>
        public int ColumnIndex { get; }

        /// <summary>
        /// Gets or sets the Text of the Cell.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (value == this.text)
                {
                    return;
                }

                this.text = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            }
        }

        /// <summary>
        /// Gets or sets the value of the cell.
        /// Value Property that represents that evaluation of a formula if text property starts with =.
        /// </summary>
        public string Value
        {
            get
            {
                return this.value;
            }

            set
            {
               this.value = value;
               this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        /// <summary>
        /// Gets or sets the background color of the cell.
        /// </summary>
        public uint BGColor
        {
            get
            {
                return this.bgColor;
            }

            set
            {
                this.bgColor = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BGColor"));
            }
        }

        /// <summary>
        /// This is the method that invokes a change in a cell that gets referenced in another cell.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">PropertyChangedEventArgs.</param>
        public void CellReferencePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));
        }
    }
}
