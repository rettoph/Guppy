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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Guppy.MonoGame
{
    public sealed class Input<T> : IInput
        where T : IMessage
    {
        private ButtonState _state;

        public string Key { get; }
        public InputSource DefaultSource { get; }

        public InputSource Source { get; set; }

        public ButtonState State => _state;

        public readonly IReadOnlyDictionary<bool, T> Data;

        public Input(
            string key,
            InputSource defaultSource,
            (bool pressed, T data)[] data)
        {
            this.Key = key;
            this.DefaultSource = defaultSource;
            this.Data = data.ToDictionary(x => x.pressed, x => x.data);

            this.Source = defaultSource;
        }

        public bool Message(bool pressed, [MaybeNullWhen(false)] out IMessage message)
        {
            if (this.Data.TryGetValue(pressed, out var data))
            {
                message = data;
                return true;
            }

            message = null;
            return false;
        }
    }
}
