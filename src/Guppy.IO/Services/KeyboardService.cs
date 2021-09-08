using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.IO.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using XnaKeyboard = Microsoft.Xna.Framework.Input.Keyboard;

namespace Guppy.IO.Services
{
    /// <summary>
    /// Simple static service instance designed to track
    /// and maintain KeyBoard related InputManager
    /// instances.
    /// </summary>
    public class KeyboardService : Asyncable
    {
        #region Private Fields
        private InputButtonService _inputButtons;
        private InputButtonManager[] _keys;
        private Int32 _keyCount;
        #endregion

        #region Public Properties
        public InputButtonManager this[Keys key] => _inputButtons[key];
        #endregion

        #region Lifecyle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _inputButtons);

            _inputButtons.OnInputManagerCreated += this.HandleInputManagerCreated;
            _keys = new InputButtonManager[((Keys[])Enum.GetValues(typeof(Keys))).Length];
        }

        protected override void PostInitialize(GuppyServiceProvider provider)
        {
            base.PostInitialize(provider);

            this.TryStart();
        }

        protected override void Release()
        {
            base.Release();

            _inputButtons = null;
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var kState = XnaKeyboard.GetState();
            for (Int32 i = 0; i < _keyCount; i++)
                this.UpdateKey(_keys[i], ref kState);
        }

        private void UpdateKey(InputButtonManager key, ref KeyboardState kState)
            => key.TrySetState(kState.IsKeyDown(key.Which.KeyboardKey) ? ButtonState.Pressed : ButtonState.Released);
        #endregion

        #region Event Handlers
        private void HandleInputManagerCreated(InputButtonService sender, InputButtonManager input)
        {
            if (input.Which.Type == InputButtonType.Keyboard)
            { // Track all created keyboard events...
                _keys[_keyCount] = input;
                _keyCount++;
            }
        }
        #endregion
    }
}
