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
    public interface ICommand : IService
    {
        #region Properties
        /// <summary>
        /// The full identifier for the current segment.
        /// </summary>
        String Phrase { get; }

        /// <summary>
        /// A list of all sub segments contained within
        /// </summary>
        IReadOnlyDictionary<String, Command> SubCommands { get; }

        /// <summary>
        /// Public sub command getter.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        Command this[String word] { get; }
        #endregion

        #region Events
        event OnCommandExecuteDelegate OnExcecute;
        #endregion


        #region Methods
        /// <summary>
        /// Add a new segment context into the current segment.
        /// </summary>
        /// <param name="context"></param>
        ICommand TryAddSubCommand(CommandContext context);

        /// <summary>
        /// Remove a subcommand from
        /// the current command
        /// </summary>
        /// <param name="word"></param>
        void TryRemove(String word);

        /// <summary>
        /// Remove a subcommand from
        /// the current command
        /// </summary>
        void TryRemove(Command segment);
        #endregion
    }
}
