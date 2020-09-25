using Guppy.IO.Commands.Delegates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Services
{
    public sealed class CommandService : SegmentBase
    {
        #region Static Fields
        public static readonly Char ArgumentIdentifier = '-';
        #endregion

        #region Public Attributes
        public override String FullIdentifier { get; } = "";
        #endregion

        #region Events
        public event OnExecuteDelegate OnExcecute;
        #endregion

        #region Helper Methods
        /// <summary>
        /// Attempt to parse a raw string & execute the command.
        /// </summary>
        /// <param name="input"></param>
        public void TryExecute(String input)
            => this.TryExecute(this.TryBuild(input));
        /// <summary>
        /// Attempt to execute a pre existing command instance.
        /// </summary>
        /// <param name="command"></param>
        public void TryExecute(Command command)
            => command.Segment.Execute(command);

        /// <summary>
        /// Attempt to construct a command instance
        /// from a recieved raw command string.
        /// 
        /// Input args will be confirmed & validated.
        /// </summary>
        /// <param name="input">The command input string</param>
        public Command TryBuild(String input)
            => this.TryBuild(input.Split(' '), 0);

        internal override Command TryBuild(string[] input, int position)
            => this[input[position]].TryBuild(input, ++position);
        #endregion

        #region SegmentBase Implementation
        internal override void Execute(Command command)
            => this.OnExcecute?.Invoke(command);
        #endregion
    }
}
