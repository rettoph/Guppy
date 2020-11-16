using Guppy.Interfaces;
using Guppy.IO.Commands.Contexts;
using Guppy.IO.Commands.Delegates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Interfaces
{
    public interface ICommandBase : IService
    {
        #region Properties
        /// <summary>
        /// A list of all sub segments contained within
        /// </summary>
        IReadOnlyDictionary<String, ICommand> Commands { get; }

        /// <summary>
        /// Public sub command getter.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        ICommand this[String word] { get; }

        /// <summary>
        /// The full identifier for the current segment.
        /// </summary>
        String Phrase { get; }
        #endregion


        #region Methods
        /// <summary>
        /// Add a new default Command into
        /// the current CommandBase using the recieved
        /// CommandContext info.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        ICommand TryAddCommand(CommandContext context);

        /// <summary>
        /// Add a new Command into
        /// the current CommandBase
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        ICommand TryAddCommand(ICommand command);

        /// <summary>
        /// Remove a subcommand from
        /// the current command
        /// </summary>
        /// <param name="word"></param>
        void TryRemove(String word);

        /// <summary>
        /// Return the help text for a recived command instance.
        /// </summary>
        /// <returns></returns>
        String GetHelpText();
        #endregion
    }
}
