using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.IO.Enums;
using Guppy.IO.Structs;
using Guppy.IO.Utilities;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using XnaMouse = Microsoft.Xna.Framework.Input.Mouse;

namespace Guppy.IO.Services
{
    /// <summary>
    /// Static service instance that will track the 
    /// current mouse activity and manage mouse events.
    /// </summary>
    public sealed class MouseService : Asyncable
    {
        #region Private Fields
        private InputButtonService _buttons;
        private Dictionary<InputButton, InputButtonManager> _mButtons;
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
        public InputButtonManager this[MouseButton button] => _mButtons[new InputButton(button)];
        #endregion

        #region Events
        public event OnEventDelegate<MouseService, ScrollWheelArgs> OnScrollWheelValueChanged;

        public event OnEventDelegate<InputButtonManager, InputButtonArgs> OnButtonStateChanged;
        public Dictionary<ButtonState, OnEventDelegate<InputButtonManager, InputButtonArgs>> OnButtonState { get; private set; }

        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _buttons);

            _mButtons = new Dictionary<InputButton, InputButtonManager>(3);
            _mButtons[new InputButton(MouseButton.Left)] = _buttons[MouseButton.Left];
            _mButtons[new InputButton(MouseButton.Middle)] = _buttons[MouseButton.Middle];
            _mButtons[new InputButton(MouseButton.Right)] = _buttons[MouseButton.Right];

            this.OnButtonState = DictionaryHelper.BuildEnumDictionary<ButtonState, OnEventDelegate<InputButtonManager, InputButtonArgs>>();

            _mButtons.Values.ForEach(i =>
            {
                i.OnStateChanged += this.HandleInputChanged;
            });
        }

        protected override void PostInitialize(GuppyServiceProvider provider)
        {
            base.PostInitialize(provider);

            // Start the async mouse updater...
            this.TryStartAsync();
        }

        protected override void Release()
        {
            base.Release();

            _buttons = null;

            _mButtons.Values.ForEach(i =>
            {
                i.OnStateChanged -= this.HandleInputChanged;
            });

            _mButtons.Clear();
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var mState = XnaMouse.GetState();

            // Save the mouse position...
            this.Position = mState.Position.ToVector2();

            // Update the mouse button states...
            this[MouseButton.Left].TrySetState(mState.LeftButton);
            this[MouseButton.Middle].TrySetState(mState.MiddleButton);
            this[MouseButton.Right].TrySetState(mState.RightButton);

            if (this.ScrollWheelValue != mState.ScrollWheelValue)
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
        private void HandleInputChanged(InputButtonManager sender, InputButtonArgs args)
        {
            this.OnButtonStateChanged?.Invoke(sender, args);
            this.OnButtonState[args.State]?.Invoke(sender, args);
        }
        #endregion
    }
}
