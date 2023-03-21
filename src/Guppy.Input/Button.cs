using Guppy.Common;
using Guppy.Input.Services;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Guppy.Input
{
    internal sealed class Button<T> : IButton
        where T : IMessage
    {
        private Microsoft.Xna.Framework.Input.ButtonState _state;

        public string Key { get; }
        public ButtonSource DefaultSource { get; }

        public ButtonSource Source { get; set; }

        public Microsoft.Xna.Framework.Input.ButtonState State => _state;

        public readonly IReadOnlyDictionary<bool, T> Data;

        public Button(
            string key,
            ButtonSource defaultSource,
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
