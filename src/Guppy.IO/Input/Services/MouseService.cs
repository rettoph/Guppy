using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Extensions.DependencyInjection;
using Guppy.IO.Enums;
using Guppy.IO.Input;
using Guppy.IO.Input.Delegates;
using Guppy.IO.Input.Enums;
using Guppy.IO.Input.Services;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Services
{
    /// <summary>
    /// Static service instance that will track the 
    /// current mouse activity & manage mouse events.
    /// </summary>
    public sealed class MouseService : Asyncable
    {
        #region Private Fields
        private InputService _input;
        private Dictionary<InputType, InputManager> _inputs;
        private Single _oldScrollWheelValue;
        #endregion

        #region Public Properties
        /// <summary>
        /// The pointers current position
        /// in screen coordinates
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// The cumulative scroll wheel values since game start.
        /// </summary>
        public Single ScrollWheelValue { get; private set; }

        /// <summary>
        /// Return a specific mouse button InputManager.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public InputManager this[MouseButton button] => _inputs[new InputType(button)];
        #endregion

        #region Events
        public event OnScrollWheelValueChangedDelegate OnScrollWheelValueChanged;

        public event OnStateChangedDelegate OnButtonStateChanged;
        public Dictionary<ButtonState, OnStateChangedDelegate> OnButtonState { get; private set; }

        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _input);

            _inputs = new Dictionary<InputType, InputManager>(3);
            _inputs[new InputType(MouseButton.Left)] = _input[MouseButton.Left];
            _inputs[new InputType(MouseButton.Middle)] = _input[MouseButton.Middle];
            _inputs[new InputType(MouseButton.Right)] = _input[MouseButton.Right];

            this.OnButtonState = DictionaryHelper.BuildEnumDictionary<ButtonState, OnStateChangedDelegate>();

            _inputs.Values.ForEach(i =>
            {
                i.OnStateChanged += this.HandleInputChanged;
            });
        }

        protected override void PostInitialize(ServiceProvider provider)
        {
            base.PostInitialize(provider);

            // Start the async mouse updater...
            this.TryStart();
        }

        protected override void Release()
        {
            base.Release();

            _input = null;

            _inputs.Values.ForEach(i =>
            {
                i.OnStateChanged -= this.HandleInputChanged;
            });

            _inputs.Clear();
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var mState = Mouse.GetState();

            // Save the mouse position...
            this.Position = mState.Position.ToVector2();

            // Update the mouse button states...
            this[MouseButton.Left].TrySetState(mState.LeftButton);
            this[MouseButton.Middle].TrySetState(mState.MiddleButton);
            this[MouseButton.Right].TrySetState(mState.RightButton);

            if(this.ScrollWheelValue != mState.ScrollWheelValue)
            { // If the scroll value has been updated...
                _oldScrollWheelValue = this.ScrollWheelValue;
                this.ScrollWheelValue = mState.ScrollWheelValue;

                this.OnScrollWheelValueChanged?.Invoke(
                    sender: this, 
                    args: new ScrollWheelArgs(
                        value: this.ScrollWheelValue, 
                        delta: this.ScrollWheelValue - _oldScrollWheelValue));
            }
            
        }
        #endregion

        #region Event Handlers
        private void HandleInputChanged(InputManager sender, InputArgs args)
        {
            this.OnButtonStateChanged?.Invoke(sender, args);
            this.OnButtonState[args.State]?.Invoke(sender, args);
        }
        #endregion
    }
}
