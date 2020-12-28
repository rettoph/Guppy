using Guppy.DependencyInjection;
using Guppy.IO.Enums;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.IO.Input.Services
{
    public sealed class InputService : Service
    {
        #region Private Fields
        /// <summary>
        /// A list of all cached inputs that
        /// should be watched.
        /// </summary>
        private Dictionary<InputType, InputManager> _inputs;
        private ServiceProvider _provider;
        #endregion

        #region Public Properties
        public InputManager this[InputType input] => this.GetManager(input);
        public InputManager this[Keys key] => this[new InputType(key)];
        public InputManager this[MouseButton button] => this[new InputType(button)];
        #endregion

        #region Events
        public delegate void OnInputManagerCreatedDelegate(InputService sender, InputManager input);

        /// <summary>
        /// Invoked when a new input manager is created.
        /// </summary>
        public event OnInputManagerCreatedDelegate OnInputManagerCreated;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _provider = provider;
            _inputs = new Dictionary<InputType, InputManager>();
        }

        protected override void Release()
        {
            base.Release();

            _provider = null;
            _inputs.Clear();
        }
        #endregion

        #region Helper Methods
        public InputManager GetManager(InputType input)
        {
            if (!_inputs.ContainsKey(input))
            { // Create a new InputManager instance...
                _inputs[input] = _provider.GetService<InputManager>((im, p, d) => im.Which = input);
                this.OnInputManagerCreated?.Invoke(this, _inputs[input]);
            }

            return _inputs[input];
        }
        #endregion
    }
}
