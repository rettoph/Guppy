using Guppy.Extensions.System;
using Guppy.IO.Commands.Delegates;
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
                response = (input, responses: input.Execute());
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
            => this.TryBuild(input.Split(' '), 0);

        internal override CommandInput TryBuild(string[] input, int position)
            => this.SubCommands.ContainsKey(input[position]) ? this[input[position]].TryBuild(input, ++position) : new CommandInput(input);
        #endregion
    }
}
