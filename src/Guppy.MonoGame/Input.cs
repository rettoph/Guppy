using Guppy.Common;
using Guppy.MonoGame.Enums;
using Guppy.MonoGame.Services;
using Guppy.MonoGame.Structs;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    public sealed class Input<TData> : IInput
        where TData : IMessage
    {
        private InputSource _source;
        private ButtonState _state;

        public string Key { get; }
        public InputSource DefaultSource { get; }

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

        public readonly IReadOnlyDictionary<ButtonState, TData> Data;

        public event OnChangedEventDelegate<IInput, InputSource>? OnSourceChanged;
        public event OnChangedEventDelegate<IInput, ButtonState>? OnStateChanged;

        public Input(
            string key,
            InputSource defaultSource,
            (ButtonState state, TData data)[] data)
        {
            this.Key = key;
            this.DefaultSource = defaultSource;
            this.Data = data.ToDictionary(x => x.state, x => x.data);

            this.Source = defaultSource;
            this.State = ButtonState.Released;
        }

        public bool Update(ref KeyboardState kState, ref MouseState mState, [MaybeNullWhen(false)] out IMessage data)
        {
            var changed = _source.Type switch
            {
                InputType.Mouse => !mState.IsState(_source.MouseButton, _state),
                InputType.Keyboard => !kState.IsState(_source.KeyboardKey, _state),
                _ => throw new NotImplementedException()
            };

            if (!changed)
            {
                data = null;
                return false;
            }

            this.State = _state == ButtonState.Pressed ? ButtonState.Released : ButtonState.Pressed;

            if(this.Data.TryGetValue(_state, out TData? message))
            {
                data = message;
                return true;
            }

            data = null;
            return false;
        }
    }
}
