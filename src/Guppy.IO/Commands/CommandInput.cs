using Guppy.Extensions.Collections;
using Guppy.IO.Commands.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Guppy.IO.Commands
{
    /// <summary>
    /// a simple container storing a specific command 
    /// invocation and the argument results.
    /// </summary>
    public sealed class CommandInput
    {
        #region Private Fields
        private String[] _input;
        private Dictionary<String, Object> _args;
        private static CommandResponse[] _unknownCommandResponse = new CommandResponse[] 
        {
            CommandResponse.Warning("Unknown command.")
        };
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
        public Object this[String identifier] { get => _args[identifier]; }

        /// <summary>
        /// The full string phrase recieved when reacting this
        /// CommandInput instance.
        /// </summary>
        public String Phrase => String.Join(" ", _input);
        #endregion

        #region Constructors
        internal CommandInput(String[] input)
        {
            _input = input;

            _args = new Dictionary<String, Object>();
        }

        internal CommandInput(Command command, String[] input, Int32 position) : this(input)
        {
            this.Command = command;

            // Build the commands dictionary...
            while (position < input.Length)
                CommandInput.AddNextArgument(command, input, ref position, ref _args);

            // Ensure that all required args were recieved...
            if (command.Arguments.Any(ac => ac.Required && !_args.ContainsKey(ac.Identifier)))
            {
                var missing = command.Arguments.Where(ac => ac.Required && !_args.ContainsKey(ac.Identifier));
                throw new MissingMemberException($"Missing arguments required: {String.Join(" ", missing.Select(ac => CommandService.ArgumentIdentifier + ac.Identifier).ToArray())}.");
            }
        }
        private CommandInput(CommandInput from)
        {
            _input = from._input;
            _args = from._args;
            this.Command = from.Command;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Self excecute the current CommandInput info
        /// & return the results.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CommandResponse> Execute()
        {
            if (this.Command == default)
                return _unknownCommandResponse;

            return this.Command.LazyExecute(this).Where(r => r != default).ToList();
        }

        public CommandInput Copy()
            => new CommandInput(this);

        /// <summary>
        /// Check whether or not a specified argument
        /// exists & is defined.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public Boolean Contains(String identifier)
            => _args.ContainsKey(identifier);

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
            => this.Phrase;
        #endregion

        #region Static Helper Methods
        /// <summary>
        /// Find the next argument identifier & its value
        /// then add it do the recieved args collection
        /// </summary>
        /// <param name="input"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private static void AddNextArgument(Command command, String[] input, ref Int32 position, ref Dictionary<String, Object> args)
        {
            if (input[position][0] != CommandService.ArgumentIdentifier)
                throw new Exception($"Unexpected argument identifier. Please use '{CommandService.ArgumentIdentifier}'");

            String identifier;
            String value;
            Int32 index;

            String next = input[position].Substring(1);

            if ((index = next.IndexOf('=')) > 0)
            { // There is an equal sign, thus no space seperator
                identifier = next.Substring(0, index);
                value = Regex.Unescape(next.Substring(++index, next.Length - index));
            }
            else
            {
                identifier = next;
                value = Regex.Unescape(input[++position]);
            }

            if (value[0] == '"')
            { // If a quote is returned...
                while (value[value.Length - 1] != '"')
                {
                    value += ' ' + Regex.Unescape(input[++position]);
                }

                value = value.Trim('"');
            }

            // Identify which argument is being returned & parse the String value
            try
            {
                var argContext = command.Arguments.First(ac => ac.Identifier == identifier || (identifier.Length == 1 && ac.Aliases.Contains(identifier[0])));
                args.Add(argContext.Identifier, argContext.Type.Parse(value));
            }
            catch (InvalidOperationException e)
            {
                throw new ArgumentException($"Unknown argument identifier or aliase '{identifier}'.");
            }

            position++;
        }
        #endregion
    }
}
