using Guppy.IO.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Delegates
{
    /// <summary>
    /// When a command gets excecuted
    /// </summary>
    /// <param name="sender">The Command currently being invoked.</param>
    /// <param name="source">The source command that srtarted the invocation.</param>
    /// <param name="args">A list of parsed arguments recieved.</param>
    public delegate void OnCommandExecuteDelegate(ICommand sender, CommandArguments args);
}
