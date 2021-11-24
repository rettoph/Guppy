using Guppy.DependencyInjection;
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
        private GuppyServiceProvider _provider;
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

        #region Lifecycle Methods
        protected override void Create(GuppyServiceProvider provider)
        {
            base.Create(provider);

            _inputCommands = new Dictionary<String, InputCommand>();
        }

        protected override void PreInitialize(GuppyServiceProvider provider)
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

        #region Helper Methods
        public InputCommand Add(InputCommandContext context)
        {
            // Validate the context...
            if (_inputCommands.ContainsKey(context.Handle))
                throw new DuplicateNameException($"Duplicate InputCommand handle detected '{context.Handle}'. Please use a unique identifier.");

            // Create a new InputCommand instance...
            var inputCommand = _provider.GetService<InputCommand>((i, s, c) =>
            {
                i.SetContext(context);
                i.InputCommandService = this;
            });

            _inputCommands.Add(inputCommand.Handle, inputCommand);

            return inputCommand;
        }
        #endregion
    }
}
