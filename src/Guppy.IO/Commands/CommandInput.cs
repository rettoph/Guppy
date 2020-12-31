using Guppy.Extensions.System.Collections;
using Guppy.Extensions.System;
using Guppy.IO.Commands.Delegates;
using Guppy.IO.Commands.Interfaces;
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
        private List<String> _invalidArgs;
        #endregion

        #region Public Properties
        /// <summary>
        /// The command the current arguments are in charge of
        /// </summary>
        public readonly ICommand Command;

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

        /// <summary>
        /// The status of the current parsed input.
        /// </summary>
        public CommandInputStatus Status { get; private set; }
        #endregion

        #region Constructors
        internal CommandInput(ICommand command, String[] input, Int32 position)
        {
            _input = input;
            _args = command.Arguments.Where(ac => ac.DefaultValue != null && !ac.Required).ToDictionary(
                keySelector: ac => ac.Identifier,
                elementSelector: ac => ac.DefaultValue?.Invoke());
            _invalidArgs = new List<String>();

            this.Command = command;
            this.Status = CommandInputStatus.Valid;

            if(this.Command == default)
            { // Ensure that the recieved command value is correct.
                this.Status = CommandInputStatus.InvalidCommand;
                return;
            }

            // Build the commands dictionary...
            while (position < input.Length)
                this.TryParseNextArgument(command, input, ref position);

            // Ensure that all required args were recieved...
            if (command.Arguments.Any(ac => ac.Required && !_args.ContainsKey(ac.Identifier)))
            {
                var missing = command.Arguments.Where(ac => ac.Required && !_args.ContainsKey(ac.Identifier));
                throw new MissingMemberException($"Missing required arguments: {String.Join(", ", missing.Select(ac => ac.Identifier))}.");
            }
        }
        private CommandInput(CommandInput from)
        {
            _input = from._input;
            _args = from._args;
            _invalidArgs = from._invalidArgs;
            this.Command = from.Command;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Return a lazy IEnumerable instance representing
        /// the current input's response.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CommandResponse> Excecute()
        {
            switch (this.Status)
            {
                case CommandInputStatus.Valid:
                    return this.Command.LazyExcecute(this).Where(r => r != CommandResponse.Empty).ToList();
                case CommandInputStatus.InvalidCommand:
                    return new CommandResponse[]
                    {
                        CommandResponse.Error($"Unknown command, type '{this.Command?.Phrase.AddRight(' ') ?? ""}help' for help.")
                    };
                case CommandInputStatus.InvalidArgument:
                    return new CommandResponse[]
                    {
                        CommandResponse.Error($"Invalid argument(s) '{String.Join(", ", _invalidArgs)}', Type '{this.Command.Phrase} help' for help.")
                    };
                default: // This should never happen. Means missing case.
                    throw new Exception($"Unknown CommandInputStatus value => {this.Status}");
            }
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
        /// <param name="fallback"></param>
        /// <returns></returns>
        public T GetIfContains<T>(String identifier, T fallback = default)
        {
            if (this.Contains(identifier))
                return (T)this[identifier];

            return fallback;
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
        private void TryParseNextArgument(ICommand command, String[] input, ref Int32 position)
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
                var argContext = command.Arguments.First(ac => ac.Identifier == identifier || (identifier.Length == 1 && (ac.Aliases?.Contains(identifier[0]) ?? false)));
                _args[argContext.Identifier] = argContext.Type.Parse(value);
            }
            catch (InvalidOperationException e)
            {
                _invalidArgs.Add(identifier);
                this.Status = CommandInputStatus.InvalidArgument;
            }

            position++;
        }
        #endregion
    }
}
