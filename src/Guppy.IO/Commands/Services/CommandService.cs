using Guppy.DependencyInjection;
using Guppy.Extensions.System;
using Guppy.IO.Commands.Contexts;
using Guppy.IO.Commands.Delegates;
using Guppy.IO.Commands.Enums;
using Guppy.IO.Commands.Extensions;
using Guppy.IO.Commands.Interfaces;
using Guppy.IO.Commands.Structs;
using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.IO.Commands.Services
{
    public sealed class CommandService : CommandBase
    {
        #region Static Fields
        public static readonly Char ArgumentIdentifier = '-';
        #endregion

        #region Public Attributes
        public override String Phrase { get; } = "";
        #endregion

        #region Events
        public event OnExecutedDelegate OnExcecuted;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.TryAddCommand(new CommandContext()
            {
                Word = "help",
                Arguments = new ArgContext[]
                {
                    new ArgContext()
                    {
                        Identifier = "command",
                        Aliases = "c".ToCharArray(),
                        Required = false,
                        Type = new ArgType(
                            name: "command",
                            parser: command => this.TryFindMatch(command).Command)
                    },
                    new ArgContext()
                    {
                        Identifier = "message",
                        Aliases = "m".ToCharArray(),
                        Required = false,
                        Type = ArgType.String
                    }
                }
            });

            this["help"].OnExcecute += (c, i) =>
            {
                return CommandResponse.Success(i.GetIfContains<String>("message")?.AddRight("\n") + (i.GetIfContains<ICommandBase>("command") ?? this).GetHelpText());
            };
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Attempt to parse a raw string & execute the command.
        /// </summary>
        /// <param name="input"></param>
        public (CommandInput input, IEnumerable<CommandResponse> responses) TryExecute(String input)
            => this.TryExecute(this.TryBuild(input));
        /// <summary>
        /// Attempt to execute a pre existing command instance.
        /// </summary>
        /// <param name="command"></param>
        public (CommandInput input, IEnumerable<CommandResponse> responses) TryExecute(CommandInput input)
        {
            (CommandInput input, IEnumerable<CommandResponse> responses) response;

            try
            {
                response = (input, responses: input.Excecute());
            }
            catch(Exception e)
            {
                response = (input, responses: new CommandResponse[]
                {
                    CommandResponse.Error("Unable to execute command.", e)
                });
            }

            return response.Then(r =>
            {
                this.OnExcecuted?.Invoke(r.input, r.responses);
            });
        }

        /// <summary>
        /// Attempt to construct a command instance
        /// from a recieved raw command string.
        /// 
        /// Input args will be confirmed & validated.
        /// </summary>
        /// <param name="input">The command input string</param>
        public CommandInput TryBuild(String input)
        {
            var position = 0;
            var inputArray = input.Split(' ');

            return this.TryFindMatch(inputArray, ref position).As(
                converter: match =>
                {
                    switch (match.Type)
                    {
                        case CommandMatchType.Incomplete:
                            return this.TryBuild($"help -command=\"{match.Command?.Phrase}\"");
                        case CommandMatchType.Complete:
                            try
                            {
                                return new CommandInput(match.Command, inputArray, position);
                            }
                            catch(Exception e)
                            {
                                return this.TryBuild($"help -command=\"{match.Command?.Phrase}\" -message=\"{e.Message}\"");
                            }
                        default: // This should never happen...
                            throw new NotImplementedException();
                    }
                });
        }

        /// <summary>
        /// helper method to search for the references command
        /// instance (if any).
        /// </summary>
        /// <param name="input"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public CommandMatch TryFindMatch(String[] input, ref Int32 position)
        {
            if (this.Commands.ContainsKey(input[position]))
                return this[input[position]].TryFindMatch(input, ref position);

            return CommandMatch.Incomplete();
        }

        public CommandMatch TryFindMatch(String input)
        {
            var position = 0;
            return this.TryFindMatch(input.Split(' '), ref position);
        }
        #endregion
    }
}
