using Guppy.DependencyInjection;
using Guppy.IO.Commands.Contexts;
using Guppy.IO.Commands.Delegates;
using Guppy.IO.Commands.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Guppy.IO.Commands
{
    public class Command : CommandBase
    {
        #region Private Fields
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
        }

        protected override void Release()
        {
            this.Parent = null;
        }

        internal override IEnumerable<CommandResponse> LazyExecute(CommandInput input)
            => base.LazyExecute(input).Concat(this.Parent.LazyExecute(input));
        #endregion

        #region Helper Methods
        public void TryAddArgument(ArgContext arg)
            => _args.Add(arg);
        public void TryRemoveArgument(ArgContext arg)
            => _args.Remove(arg);

        public CommandInput BuildInput(String[] input, Int32 position)
            => new CommandInput(this, input, position);
        #endregion

        #region SegmentBase Implementation
        internal override CommandInput TryBuild(String[] input, Int32 position)
        {
            if (position == input.Length || input[position][0] == CommandService.ArgumentIdentifier)
            { // We have hit the first argument... attempt to parse them & build the command.
                return this.BuildInput(input, position);
            }
            else
            { // There is a sub segment requested...
                return this[input[position]].TryBuild(input, ++position);
            }
        }

        #endregion
    }
}
