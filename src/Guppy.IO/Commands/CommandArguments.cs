using Guppy.Extensions.Collections;
using Guppy.IO.Commands.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.IO.Commands
{
    /// <summary>
    /// a simple container storing a specific command 
    /// invocation and the argument results.
    /// </summary>
    public sealed class CommandArguments
    {
        #region Internal Properties
        internal Dictionary<String, Object> args { get; private set; }
        #endregion

        #region Public Properties
        /// <summary>
        /// The command the current arguments are in charge of
        /// </summary>
        public readonly Command Command;

        /// <summary>
        /// Grab a specific command by name
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public Object this[String identifier] { get => this.args[identifier]; }
        #endregion

        #region Constructors
        internal CommandArguments(Command command)
        {
            this.args = new Dictionary<String, Object>();

            this.Command = command;
        }
        private CommandArguments(Command command, Dictionary<String, Object> copyFrom) : this(command)
        {
            copyFrom.ForEach(kvp => this.args.Add(kvp.Key, kvp.Value));
        }
        #endregion

        public CommandArguments Copy()
            => new CommandArguments(this.Command, this.args);

        /// <summary>
        /// Check whether or not a specified argument
        /// exists & is defined.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public Boolean Contains(String identifier)
            => this.args.ContainsKey(identifier);

        /// <summary>
        /// If the requested argument exists return
        /// it, otherwise return null
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public T GetIfContains<T>(String identifier)
        {
            if (this.Contains(identifier))
                return (T)this[identifier];

            return default(T);
        }

        public override String ToString()
        {
            return this.Command.Phrase + String.Join("", this.args.Select(kvp => $" {CommandService.ArgumentIdentifier}{kvp.Key}={kvp.Value}"));
        }
    }
}
