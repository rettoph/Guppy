using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.IO.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Guppy.IO.Services
{
    /// <summary>
    /// Simple service used to translate input events
    /// into command excecutions.
    /// </summary>
    public sealed class InputCommandService : Service
    {
        #region Private Fields
        private ServiceProvider _provider;
        private Dictionary<String, InputCommand> _inputCommands;
        #endregion

        #region Public Properties
        public InputCommand this[String handle] => _inputCommands[handle];

        /// <summary>
        /// When locked, inputs will no longer activate commands.
        /// This may be used when user inputs should be halted.
        /// </summary>
        public Boolean Locked { get; set; }
        #endregion

        #region Constructor
        internal InputCommandService(Dictionary<String, InputCommand> inputCommands)
        {
            _inputCommands = inputCommands;

            foreach(InputCommand inputCommand in _inputCommands.Values)
            {
                inputCommand.InputCommandService = this;
            }
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            _inputCommands = new Dictionary<String, InputCommand>();
        }

        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _provider = provider;
        }

        protected override void Release()
        {
            base.Release();

            _provider = null;
            _inputCommands.ForEach(ic => ic.Value.TryRelease());
            _inputCommands.Clear();
        }
        #endregion
    }
}
