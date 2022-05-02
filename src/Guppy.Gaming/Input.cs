using Guppy.Gaming.Enums;
using Guppy.Gaming.Services;
using Guppy.Gaming.Structs;
using Guppy.Threading;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming
{
    public sealed class Input
    {
        private InputSource _source;
        private ButtonState _state;
        private readonly ICommandService _commands;

        public readonly string Key;
        public readonly InputSource DefaultSource;
        private readonly Action<IServiceProvider, ICommandService> _publishPress;
        private readonly Action<IServiceProvider, ICommandService> _publishRelease;

        public InputSource Source
        {
            get => _source;
            set
            {
                this.OnSourceChanged!.InvokeIf(value != _source, this, ref _source, value);
            }
        }
        public ButtonState State
        {
            get => _state;
            set
            {
                this.OnStateChanged!.InvokeIf(value != _state, this, ref _state, value);
            }
        }

        public event OnChangedEventDelegate<Input, InputSource>? OnSourceChanged;
        public event OnChangedEventDelegate<Input, ButtonState>? OnStateChanged;

        public Input(
            string key, 
            InputSource defaultSource, 
            Action<IServiceProvider, ICommandService> publishPress, 
            Action<IServiceProvider, ICommandService> publishRelease,
            ICommandService commands)
        {
            _publishPress = publishPress;
            _publishRelease = publishRelease;
            _commands = commands;

            this.Key = key;
            this.DefaultSource = defaultSource;

            this.Source = defaultSource;
            this.State = ButtonState.Released;
        }

        internal void UpdateState(IServiceProvider provider, ref KeyboardState kState, ref MouseState mState)
        {
            var changed = _source.Type switch
            {
                InputType.Mouse => !mState.IsState(_source.MouseButton, _state),
                InputType.Keyboard => !kState.IsState(_source.KeyboardKey, _state),
                _ => throw new NotImplementedException()
            };

            if(!changed)
            {
                return;
            }

            this.State = _state == ButtonState.Pressed ? ButtonState.Released : ButtonState.Pressed;

            switch (_state)
            {
                case ButtonState.Released:
                    _publishRelease(provider, _commands);
                    break;
                case ButtonState.Pressed:
                    _publishPress(provider, _commands);
                    break;
            }
        }
    }
}
