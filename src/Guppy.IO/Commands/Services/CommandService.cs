using Guppy.DependencyInjection;
using Guppy.IO.Commands.Extensions;
using Guppy.IO.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Services
{
    public sealed class CommandService : Service, ICommandGroupContextParent
    {
        #region Static Attributes
        public static Char ArgumentIdentifier { get; } = '-';
        #endregion

        #region Private Attributes
        private Dictionary<String, ICommandGroupContext> _groups;
        #endregion

        #region Public Attributes
        /// <inheritdoc />
        public IReadOnlyDictionary<String, ICommandGroupContext> Groups => _groups;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _groups = new Dictionary<String, ICommandGroupContext>();
        }
        #endregion

        #region Helper Methods
        public void Excecute(String command)
            => this.Excecute(this.Build(command));

        public Command Build(String command)
            => this.Build(command.Split(' '), 0);

        public void Excecute(Command command)
        {

        }

        public void Add(ICommandGroupContext context)
        {
            if (context.TrySetParent(null))
                _groups.Add(context.Name, context);
        }

        public void Remove(ICommandGroupContext context)
        {
            if (this.Groups.ContainsKey(context.Name) && context.TrySetParent(null))
                _groups.Remove(context.Name);
        }
        #endregion
    }
}
