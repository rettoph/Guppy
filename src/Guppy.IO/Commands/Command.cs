using Guppy.DependencyInjection;
using Guppy.IO.Commands.Contexts;
using Guppy.IO.Commands.Delegates;
using Guppy.IO.Commands.Extensions;
using Guppy.IO.Commands.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Guppy.IO.Commands
{
    public class Command : CommandBase
    {
        #region Private Fields
        private CommandArguments _parsedArgs;
        private List<ArgContext> _args;
        #endregion

        #region Public Properties
        /// <summary>
        /// The owning parent segment base.
        /// </summary>
        public CommandBase Parent { get; internal set; }

        /// <inheritdoc />
        public override String Phrase => (this.Parent.Phrase + ' ' + this.Word).TrimStart();

        /// <summary>
        /// The local segment identifier
        /// </summary>
        public String Word { get; internal set; }

        public IReadOnlyCollection<ArgContext> Arguments => _args;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _args = new List<ArgContext>();
            _parsedArgs = new CommandArguments(this);
        }

        protected override void Release()
        {
            this.Parent = null;
        }

        internal override void Execute(CommandArguments arguments)
        {
            this.Parent.Execute(arguments);

            base.Execute(arguments);
        }
        #endregion

        #region Helper Methods
        public void TryAddArgument(ArgContext arg)
            => _args.Add(arg);
        public void TryRemoveArgument(ArgContext arg)
            => _args.Remove(arg);
        #endregion

        #region SegmentBase Implementation
        internal override CommandArguments TryBuild(String[] input, Int32 position)
        {
            if (position == input.Length || input[position][0] == CommandService.ArgumentIdentifier)
            { // We have hit the first argument... attempt to parse them & build the command.
                this.ParseArguments(input, position, _parsedArgs.args);
                return _parsedArgs;
            }
            else
            { // There is a sub segment requested...
                return this[input[position]].TryBuild(input, ++position);
            }
        }
        #endregion
    }
}
