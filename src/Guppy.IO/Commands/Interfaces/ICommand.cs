using Guppy.Interfaces;
using Guppy.IO.Commands.Contexts;
using Guppy.IO.Commands.Delegates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Interfaces
{
    /// <summary>
    /// Primary interface used to define global command functionality
    /// </summary>
    public interface ICommand : ICommandBase
    {
        #region Properties
        /// <summary>
        /// The primary parent whithin which the 
        /// current command resides, as commands
        /// can exist within multiple parents.
        /// 
        /// For example, the help command.
        /// </summary>
        ICommandBase PrimaryParent { get; }

        /// <summary>
        /// The local segment identifier
        /// </summary>
        String Word { get; }

        /// <summary>
        /// The arguments configured to the current command.
        /// </summary>
        IReadOnlyCollection<ArgContext> Arguments { get; }

        /// <summary>
        /// A human readable description of the current commands
        /// intended functionality.
        /// </summary>
        String Description { get; }
        #endregion

        #region Events
        event OnCommandExecuteDelegate OnExcecute;
        #endregion

        #region Methods
        /// <summary>
        /// Return a lazy IEnumerable instance representing
        /// the current input's response.
        /// </summary>
        /// <returns></returns>
        IEnumerable<CommandResponse> LazyExcecute(CommandInput input);
        #endregion
    }
}
