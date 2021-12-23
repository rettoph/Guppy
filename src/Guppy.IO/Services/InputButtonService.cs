using Guppy.IO.Enums;
using Guppy.IO.Utilities;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.IO.EventArgs;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent;
using Guppy.IO.Structs;

namespace Guppy.IO.Services
{
    public sealed class InputButtonService : Service
    {
        #region Private Fields
        /// <summary>
        /// A list of all cached inputs that
        /// should be watched.
        /// </summary>
        private Dictionary<InputButton, InputButtonManager> _inputs;
        private ServiceProvider _provider;
        #endregion

        #region Public Properties
        public InputButtonManager this[InputButton input] => this.GetManager(input);
        public InputButtonManager this[Keys key] => this[new InputButton(key)];
        public InputButtonManager this[MouseButton button] => this[new InputButton(button)];
        #endregion

        #region Events
        /// <summary>
        /// Invoked when a new input manager is created.
        /// </summary>
        public event OnEventDelegate<InputButtonService, InputButtonManager> OnInputManagerCreated;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _provider = provider;
            _inputs = new Dictionary<InputButton, InputButtonManager>();
        }

        protected override void Release()
        {
            base.Release();

            _provider = null;
            _inputs.Clear();
        }
        #endregion

        #region Helper Methods
        public InputButtonManager GetManager(InputButton input)
        {
            if (!_inputs.ContainsKey(input))
            { // Create a new InputManager instance...
                _inputs[input] = _provider.GetService<InputButtonManager>((im, p, d) => im.Which = input);
                this.OnInputManagerCreated?.Invoke(this, _inputs[input]);
            }

            return _inputs[input];
        }
        #endregion
    }
}
