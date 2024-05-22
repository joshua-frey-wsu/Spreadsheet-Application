// <copyright file="Form1.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Joshua_Frey
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement;

    /// <summary>
    /// This is the form class which handles the front end/UI of our spreadsheet application.
    /// </summary>
    public partial class Form1 : Form
    {
        private Spreadsheet spreadsheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// This is our constructor for our Windows Form which initalizes our components.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// This is our method for when the datagridview gets initialized.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">DataGridViewCellEventArgs.</param>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.InitializeDataGrid(); // When the form loads it initializes the dataGridView
            this.spreadsheet = new Spreadsheet(50, 26); // Creates new spreadsheet to handle logic for datagridview
            this.UpdateUndoRedoButtons(); // update the enability of the undo and redo buttons
            this.dataGridView1.CellBeginEdit += this.dataGridView1_CellBeginEdit;
            this.dataGridView1.CellEndEdit += this.dataGridView1_CellEndEdit;
            this.spreadsheet.CellPropertyChanged += this.HandleCellChange;
        }

        private void InitializeDataGrid()
        {
            this.Controls.Add(this.dataGridView1);

            // Adds 26 columns to the dataGridView
            this.dataGridView1.ColumnCount = 26;
            for (int i = 0; i < 26; i++)
            {
                this.dataGridView1.Columns[i].HeaderText = Convert.ToString(Convert.ToChar(i + 65)); // Sets the HeaderText of the columns to be A-Z
            }

            // Adds 50 rows to the dataGridView
            this.dataGridView1.RowCount = 50;
            for (int i = 0; i < 50; i++)
            {
                this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString(); // sets the headercell value for each row to be from 1 to 50
            }

            this.dataGridView1.DefaultCellStyle.BackColor = Color.White; // sets all the cells to be a background of white
        }

        private void HandleCellChange(object sender, PropertyChangedEventArgs e)
        {
            if (sender is Cell)
            {
                // In the case that the text property changed in a cell.
                if (e.PropertyName.CompareTo("Text") == 0)
                {
                    this.dataGridView1.Rows[((Cell)sender).RowIndex].Cells[((Cell)sender).ColumnIndex].Value = ((Cell)sender).Value;
                }

                // In the case that the BGColor property changed in a cell.
                else if (e.PropertyName.CompareTo("BGColor") == 0)
                {
                    this.dataGridView1.Rows[((Cell)sender).RowIndex].Cells[((Cell)sender).ColumnIndex].Style.BackColor = Color.FromArgb((int)((Cell)sender).BGColor);
                }
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Cell cell = this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex); // gets the current cell
            string oldText = cell.Text; // gets the current tex t of the cell before it gets changed
            string newText = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(); // this is what the new text of the cell will be
            ChangeCellTextCommand changeCellTextCommand = new ChangeCellTextCommand(cell, oldText, newText); // Creates a new command with the cell being changed, its old text and its new text
            this.spreadsheet.AddUndo(changeCellTextCommand); // adds the command to the undo list.
            this.UpdateUndoRedoButtons(); // updates the enability of the redo and undo buttons
            this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(); // updates spreadsheet
            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Value; // updates datagridview
        }

        private void Demo_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            string randText = "I love C#!";

            int randRow, randCol;

            for (int i = 0; i < 50; i++)
            {
                randRow = rand.Next(50);
                randCol = rand.Next(26);
                this.spreadsheet.GetCell(randRow, randCol).Text = randText;
            }

            for (int i = 0; i < this.spreadsheet.RowCount; i++)
            {
                this.spreadsheet.GetCell(i, 1).Text = "This is cell B" + (i + 1).ToString();
                this.spreadsheet.GetCell(i, 0).Text = "=B" + (i + 1).ToString();
            }
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            // Checks to see if there is at least 1 cell in the datagrid that is selected to be changed in color
            if (this.dataGridView1.SelectedCells.Count > 0)
            {
                ColorDialog myDialog = new ColorDialog();
                List<(Cell, uint)> previousCellColors = new List<(Cell, uint)>();

                // Keeps the user from selecting a custom color.
                myDialog.AllowFullOpen = false;

                // Allows the user to get help. (The default is false.)
                myDialog.ShowHelp = true;

                // Sets the initial color select to the current text color.
                myDialog.Color = this.Cell.ForeColor;

                // Update the text box color if the user clicks OK
                if (myDialog.ShowDialog() == DialogResult.OK)
                {
                    // Goes through each selected cell in the datagridview
                    foreach (DataGridViewCell dataGridViewSelectedCell in this.dataGridView1.SelectedCells)
                    {
                        // Gets the current row and columnindex of the selected cell.
                        int rowIndex = dataGridViewSelectedCell.RowIndex;
                        int columnIndex = dataGridViewSelectedCell.ColumnIndex;

                        // adds the current cell getting changed and its background color as a tuple in a list
                        previousCellColors.Add((this.spreadsheet.GetCell(rowIndex, columnIndex), this.spreadsheet.GetCell(rowIndex, columnIndex).BGColor));

                        // Sets the new BGColor property of the cell in the spreadsheet based on what the user selects from the colordialog box.
                        this.spreadsheet.GetCell(rowIndex, columnIndex).BGColor = ((uint)myDialog.Color.A << 24) | ((uint)myDialog.Color.R << 16) | ((uint)myDialog.Color.G << 8) | ((uint)myDialog.Color.B << 0); // Converts from System.Drawing.Color to uint
                    }

                    // Adds the list of tuples as a parameter in the command and adds that command to the undo list.
                    ChangeCellBGColorCommand changeCellBGColorCommand = new ChangeCellBGColorCommand(previousCellColors, ((uint)myDialog.Color.A << 24) | ((uint)myDialog.Color.R << 16) | ((uint)myDialog.Color.G << 8) | ((uint)myDialog.Color.B << 0));
                    this.spreadsheet.AddUndo(changeCellBGColorCommand);
                    this.UpdateUndoRedoButtons();
                }
            }
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            // If the undo stack isn't empty then perform a undo command then update the enability of the buttons.
            if (this.spreadsheet.CanUndo)
            {
                this.spreadsheet.Undo();
                this.UpdateUndoRedoButtons();
            }
        }

        private void RedoButton_Click(object sender, EventArgs e)
        {
            // If the redo stack isn't empty then perform a redo command then update the enability of the buttons.
            if (this.spreadsheet.CanRedo)
            {
                this.spreadsheet.Redo();
                this.UpdateUndoRedoButtons();
            }
        }

        private void UpdateUndoRedoButtons()
        {
            // Enable or disable undo and redo buttons based on the state of undo and redo stacks
            this.UndoButton.Enabled = this.spreadsheet.CanUndo;
            this.RedoButton.Enabled = this.spreadsheet.CanRedo;
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Uses an openfiledialog to be able to open a XML file from the users computer.
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML File|*.xml";
            openFileDialog.Title = "Open an XML File.";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != string.Empty)
            {
                System.IO.FileStream fs = (System.IO.FileStream)openFileDialog.OpenFile();

                this.spreadsheet.Load(fs); // applies the engine logic of loading the XML file to the spreadsheet.

                fs.Close(); // closes the filestream
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Uses an savefiledialog to be able to save the current state of the spreadsheet into an XML file in the users computer.
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML File|*.xml";
            saveFileDialog.Title = "Open an XML File.";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != string.Empty)
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();

                this.spreadsheet.Save(fs); // applies the engine logic of saving the state of the spreadsheet

                fs.Close(); // closes the filestream
            }
        }
    }
}