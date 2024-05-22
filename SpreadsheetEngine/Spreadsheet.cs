// <copyright file="Spreadsheet.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Joshua_Frey
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// This class handles the logic of the datagridview in the frontend using a spreadsheet represented by a 2D array of Cells.
    /// </summary>
    public class Spreadsheet
    {
        private Cell[,] cellArray;

        private Stack<ICommand> undos = new Stack<ICommand>();

        private Stack<ICommand> redos = new Stack<ICommand>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// This is the constructor of our spreadsheet class.
        /// The constructor initializes all the properties and subscribes each cell in the spreadsheet to the CellPropertyChangedEvent.
        /// </summary>
        /// <param name="numRows">The number of rows in the spreadsheet.</param>
        /// <param name="numColumns">The number of columns in the spreadsheet.</param>
        public Spreadsheet(int numRows, int numColumns)
        {
            this.RowCount = numRows;
            this.ColumnCount = numColumns;
            this.cellArray = new Cell[numRows, numColumns];

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numColumns; j++)
                {
                    this.cellArray[i, j] = new TextCell(i, j);
                    this.cellArray[i, j].PropertyChanged += this.CellPropertyChangedEvent;
                }
            }
        }

        /// <summary>
        /// This is a event handler for when the property of a cell changes in the spreadsheet.
        /// </summary>
        public event PropertyChangedEventHandler CellPropertyChanged = (sender, e) => { };

        /// <summary>
        /// Gets or sets the number of rows in the spreadsheet.
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// Gets or sets the number of columns in the spreadsheet.
        /// </summary>
        public int ColumnCount { get; set; }

        /// <summary>
        /// Gets a value indicating whether if the the undos stack is not empty.
        /// </summary>
        public bool CanUndo => this.undos.Count > 0;

        /// <summary>
        /// Gets a value indicating whether if the redos stack is not empty.
        /// </summary>
        public bool CanRedo => this.redos.Count > 0;

        /// <summary>
        /// This function returns the Cell at the row and column inputted in the parameter.
        /// </summary>
        /// <param name="rowIndex">The row index of the Cell.</param>
        /// <param name="columnIndex">The column index of the Cell.</param>
        /// <returns>Cell.</returns>
        public Cell GetCell(int rowIndex, int columnIndex)
        {
            // Checks to make sure that the provided rowIndex and columnIndex is within the range of the actual total row count and total column count
            if ((rowIndex >= 0 && rowIndex < this.RowCount) && (columnIndex >= 0 && columnIndex < this.ColumnCount))
            {
                return this.cellArray[rowIndex, columnIndex]; // returns the specified cell
            }

            return null; // else returns null
        }

        /// <summary>
        /// Adds a command into the undo stack.
        /// </summary>
        /// <param name="command">The command being pushed to the undo stack.</param>
        public void AddUndo(ICommand command)
        {
            this.undos.Push(command);
        }

        /// <summary>
        /// Implements a Undo feature for the last command.
        /// </summary>
        public void Undo()
        {
            // if undo stack is empty then don't do anything
            if (this.undos.Count > 0)
            {
                ICommand undoCommand = this.undos.Pop(); // returns the lastest command in the undos stack
                undoCommand.Undo(); // undo the command using the command interface
                this.redos.Push(undoCommand); // Every undo that is executed should automatically push an item onto the redo stack
            }
        }

        /// <summary>
        /// Implemetns a Redo feature for the latest command.
        /// </summary>
        public void Redo()
        {
            if (this.redos.Count > 0)
            {
                ICommand redoCommand = this.redos.Pop(); // returns the lastest command in the redos stack
                redoCommand.Execute(); // redo the command using the command interface
                this.undos.Push(redoCommand); // Every redo that is executed should automatically push an item onto the undo stack
            }
        }

        /// <summary>
        /// Loads an XML file that contains the properties of the spreadsheet and its cells and applies it to the spreadsheet.
        /// </summary>
        /// <param name="stream">Takes a stream to read the XML file.</param>
        public void Load(Stream stream)
        {
            // Clear the spreadsheet first before loading.
            for (int i = 0; i < this.RowCount; i++)
            {
                for (int j = 0; j < this.ColumnCount; j++)
                {
                    Cell cell = this.GetCell(i, j); // gets each cell in the spreadsheet

                    // set each cell to default properties
                    cell.Text = string.Empty;
                    cell.Value = string.Empty;
                    cell.BGColor = 0xFFFFFFFF;
                }
            }

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = false;

            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                while (reader.Read())
                {
                    // finds the cell tag
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "cell")
                    {
                        // traverses through each cell tag attribute until it reaches the name attribute
                        int i = 0;
                        reader.MoveToAttribute(i);
                        while (reader.Name != "name")
                        {
                            i++;
                            reader.MoveToAttribute(i); // moves to the next attribute
                        }

                        string cellName = reader.GetAttribute("name"); // gets the name attribute for the cellName.
                        int colIndex = (int)cellName[0] - 65; // Assumes the first is A-Z value and convert it to the actualy index (int)
                        int rowIndex = Convert.ToInt32(cellName.Substring(1)) - 1; // the next item in the text will be the rowNumber so get the substring then minus 1 for 0-based indexing
                        Cell getFormulatedCell = this.GetCell(rowIndex, colIndex); // uses the colIndex and rowIndex to get the actual Cell

                        while (reader.Read())
                        {
                            // reads the element tags
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                // reads the element content as a string and sets each cell property as that, if it reads an unused tag then it skips it.
                                switch (reader.Name)
                                {
                                    case "text":
                                        getFormulatedCell.Text = reader.ReadElementContentAsString();
                                        break;
                                    case "bgcolor":
                                        getFormulatedCell.BGColor = Convert.ToUInt32(reader.ReadElementContentAsString());
                                        break;
                                    default:
                                        reader.Skip();
                                        break;
                                }
                            }

                            // gets the end cell tag
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "cell")
                            {
                                break;
                            }
                        }
                    }
                }

                reader.Close();
            }

            // Clear thh undo and redo stack after loading.
            this.redos.Clear();
            this.undos.Clear();
        }

        /// <summary>
        /// Writes the current state of the spreadsheet and saves it to an XML file.
        /// </summary>
        /// <param name="stream">Uses a stream to write to an XML file.</param>
        public void Save(Stream stream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Async = false;
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                writer.WriteStartDocument(); // starts a xml document
                writer.WriteStartElement("spreadsheet"); // spreadsheet is the root element

                // Goes through each cell in the spreadsheet
                for (int i = 0; i < this.RowCount; i++)
                {
                    for (int j = 0; j < this.ColumnCount; j++)
                    {
                        Cell cell = this.GetCell(i, j); // gets each cell
                        string cellName = Convert.ToChar(j + 65) + Convert.ToString(i + 1); // converts the index of the cells to get the cellName

                        // If at least one of the properties isn't its defualt property setting then it has changed and will be written in the XML
                        if (cell.Text != string.Empty || cell.BGColor != 0xFFFFFFFF)
                        {
                            writer.WriteStartElement("cell"); // writes the cell element tag
                            writer.WriteAttributeString("name", cellName); // writes the name of the cell as an attribute of the cell tag

                            // if the cell isn't empty or null then write to the text property of the cell as the new text element tag.
                            if (cell.Text != null || cell.Text != string.Empty)
                            {
                                writer.WriteElementString("text", cell.Text);
                            }

                            // if the cell bgcolor property isnt the default color then write to the bgcolor property of the cell as the new bgcolor element tag.
                            if (cell.BGColor != 0xFFFFFFFF)
                            {
                                writer.WriteElementString("bgcolor", cell.BGColor.ToString());
                            }

                            writer.WriteEndElement(); // closes the text or bgcolor tag for writing
                        }
                    }
                }

                // closes spreadsheet element, documents, and writer
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
        }

        private void CellPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            // Get the cell that has changed.
            Cell cell = sender as Cell;

            // If the cell is null, exit the event handler.
            if (cell == null)
            {
                return;
            }

            // Get the property that has changed.
            string propertyName = e.PropertyName;

            // If the property that has changed is "Text", recalculate the value of the cell.
            if (propertyName == "Text")
            {
                // if the Text starts with = then evaluate the value of the formulated cell and the text is not an empty string
                if (cell.Text != string.Empty && cell.Text[0] == '=')
                {
                    string formula = cell.Text.Substring(1); // Gets all of the text on the right of the = which will be the formula

                    // In the case that it is a simple reference to one cell and no operations.
                    if (!formula.Contains('+') && !formula.Contains('-') && !formula.Contains('*') && !formula.Contains('/') && !formula.Contains('(') && !formula.Contains(')'))
                    {
                        int colIndex = (int)cell.Text[1] - 65; // Assumes the first is A-Z value and convert it to the actualy index (int)
                        int rowIndex;

                        // checks to see if the cellName rowIndex portion is a proper number and not a bad cell name.
                        if (int.TryParse(cell.Text.Substring(2), out rowIndex))
                        {
                            rowIndex--;
                        }
                        else
                        {
                            cell.Value = "!(bad reference)";
                            this.CellPropertyChanged?.Invoke(cell, e);
                            return;
                        }

                        // Checks to see if it is referencing itself, it it is then set error message as cell value
                        if (rowIndex == cell.RowIndex && colIndex == cell.ColumnIndex)
                        {
                            cell.Value = "!(self-reference)";
                            this.CellPropertyChanged?.Invoke(cell, e);
                            return;
                        }

                        // uses the colIndex and rowIndex to get the actual Cell.
                        // It will check to see if the cellName is out of range of the spreadsheet or if the first character in the cellName is a Capital
                        Cell getFormulatedCell = this.GetCell(rowIndex, colIndex);

                        // if the cell is null or if the cell value is empty then set the cell value to a error message.
                        if (getFormulatedCell == null)
                        {
                            cell.Value = "!(bad reference)";
                        }
                        else
                        {
                            cell.SubscribedCells.Add(getFormulatedCell);

                            if (getFormulatedCell.Value == string.Empty)
                            {
                                getFormulatedCell.PropertyChanged += cell.CellReferencePropertyChanged;
                                cell.Value = "0";
                            }
                            else if (getFormulatedCell.SubscribedCells.Contains(cell))
                            {
                                cell.Value = "!(circular reference)";
                            }
                            else
                            {
                                // Subscribes the variable/cell referenced in the formula to the current cell in the case that the referenced cell changes it also updates this current cell
                                getFormulatedCell.PropertyChanged += cell.CellReferencePropertyChanged;
                                cell.Value = getFormulatedCell.Value; // set the value of the cell that is being formulated to the one you're setting it to
                            }
                        }

                        // Raise the CellPropertyChanged event.
                        this.CellPropertyChanged?.Invoke(cell, e);
                        return;
                    }

                    ExpressionTree expressionTree = new ExpressionTree(formula); // Creates an expression tree with the formula we obtained

                    List<string> variableNames = expressionTree.GetVariableNames(); // Gets a list of the variable names inside of the expression tree

                    // Goes through each variable and the list of variables and gets its cell value to see if it is empty or has a value.
                    foreach (var varName in variableNames)
                    {
                        int colIndex = (int)varName[0] - 65; // Assume first letter in Variable is A-Z and convert it to the acutal index in the spreadsheet
                        int rowIndex;

                        // checks to see if the variableName's rowIndex portion is a proper number and not a bad cell name.
                        if (int.TryParse(varName.Substring(1), out rowIndex))
                        {
                            rowIndex--; // For 0-based indexing
                        }
                        else
                        {
                            cell.Value = "!(bad reference)";
                            this.CellPropertyChanged?.Invoke(cell, e);
                            return;
                        }

                        // Checks to see if it is referencing itself, it it is then set error message as cell value
                        if (rowIndex == cell.RowIndex && colIndex == cell.ColumnIndex)
                        {
                            cell.Value = "!(self-reference)";
                            this.CellPropertyChanged?.Invoke(cell, e);
                            return;
                        }

                        Cell varCell = this.GetCell(rowIndex, colIndex); // Gets the cell of the current variable

                        // if the cell is null then set the cell value to a error message.
                        if (varCell == null)
                        {
                            cell.Value = "!(bad reference)";
                            this.CellPropertyChanged?.Invoke(cell, e);
                            return;
                        }
                        else if (varCell.Value == string.Empty)
                        {
                            cell.Value = "0";
                        }

                        cell.SubscribedCells.Add(varCell);
                        if (varCell.SubscribedCells.Contains(cell))
                        {
                            cell.Value = "!(circular reference)";
                            this.CellPropertyChanged?.Invoke(cell, e);
                            return;
                        }

                        double varValue;

                        // Tries to parse the value of the current cell value to see if it can be turned into a double
                        if (double.TryParse(varCell.Value, out varValue))
                        {
                            expressionTree.SetVariable(varName, varValue); // Sets the value of the variable name as the value of that cell
                        }

                        // Subscribes the variable/cell referenced in the formula to the current cell in the case that the referenced cell changes it also updates this current cell
                        if (varCell != null)
                        {
                            varCell.PropertyChanged += cell.CellReferencePropertyChanged;
                        }
                    }

                    string result = expressionTree.Evaluate().ToString(); // Evaluates the value of the formula using the expression tree
                    cell.Value = result; // Sets the new value of the cell
                }
                else
                {
                    cell.Value = cell.Text; // if the text doesn't start with = then set the value to be the text
                }
            }

            // Raise the CellPropertyChanged event.
            this.CellPropertyChanged?.Invoke(cell, e);
        }
    }
}
