using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Extensions.DependencyInjection;
using Guppy.IO.Input.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Input.Services
{
    /// <summary>
    /// Simple static service instance designed to track
    /// and maintain KeyBoard related InputManager
    /// instances.
    /// </summary>
    public class KeyboardService : Asyncable
    {
        #region Private Fields
        private InputService _inputs;
        private InputManager[] _keys;
        private Int32 _keyCount;
        #endregion

        #region Lifecyle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _inputs);

            _inputs.OnInputManagerCreated += this.HandleInputManagerCreated;
            _keys = new InputManager[((Keys[])Enum.GetValues(typeof(Keys))).Length];
        }

        protected override void PostInitialize(ServiceProvider provider)
        {
            base.PostInitialize(provider);

            this.TryStart();
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var kState = Keyboard.GetState();
            for (Int32 i = 0; i < _keyCount; i++)
                this.UpdateKey(_keys[i], kState);
        }

        private void UpdateKey(InputManager key, KeyboardState kState)
            => key.TrySetState(kState.IsKeyDown(key.Which.KeyboardKey) ? ButtonState.Pressed : ButtonState.Released);
        #endregion

        #region Event Handlers
        private void HandleInputManagerCreated(InputService sender, InputManager input)
        {
            if(input.Which.Type == InputTypeType.Keyboard)
            { // Track all created keyboard events...
                _keys[_keyCount] = input;
                _keyCount++;
            }
        }
        #endregion
    }
}
