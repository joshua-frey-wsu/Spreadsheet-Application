// <copyright file="ICommand.cs" company="PlaceholderCompany">
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
    /// public =interface for all commands.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// A command is invoked through its execute method.
        /// </summary>
        void Execute();

        /// <summary>
        /// Undo command is invoked through its Undo method.
        /// </summary>
        void Undo();
    }
}
