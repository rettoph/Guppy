using Guppy.DependencyInjection;
using Guppy.IO.Commands.Delegates;
using Guppy.IO.Commands.Extensions;
using Guppy.IO.Commands.Interfaces;
using Guppy.IO.Commands.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands
{
    public class Segment : SegmentBase
    {
        #region Private Fields
        private Dictionary<String, Object> _parsedArgs;
        #endregion

        #region Public Properties
        /// <summary>
        /// The owning parent segment base.
        /// </summary>
        public SegmentBase Parent { get; internal set; }

        /// <inheritdoc />
        public override String FullIdentifier => (this.Parent.FullIdentifier + ' ' + this.Identifier).TrimStart();

        /// <summary>
        /// The local segment identifier
        /// </summary>
        public String Identifier { get; internal set; }

        /// <summary>
        /// The CommandContext to excecute if the current segment
        /// is invoked.
        /// </summary>
        public ICommandContext CommandContext { get; internal set; }
        #endregion

        #region Events
        public event OnSegmentExecuteDelegate OnExcecute;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _parsedArgs = new Dictionary<String, Object>();
        }

        protected override void Release()
        {
            this.Parent = null;
            this.CommandContext = null;
        }

        internal override void Execute(Command command)
        {
            this.Parent.Execute(command);

            this.OnExcecute?.Invoke(this, command);
        }
        #endregion

        #region SegmentBase Implementation
        internal override Command TryBuild(String[] input, Int32 position)
        {
            if (position == input.Length || input[position][0] == CommandService.ArgumentIdentifier)
            { // We have hit the first argument... attempt to parse them & build the command.
                return new Command(this, this.CommandContext.GetOutput(input, position, _parsedArgs));
            }
            else
            { // There is a sub segment requested...
                return this[input[position]].TryBuild(input, ++position);
            }
        }
        #endregion
    }
}
