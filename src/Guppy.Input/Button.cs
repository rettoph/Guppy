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
        public string Key { get; }

        public ButtonSource DefaultSource { get; }

        public ButtonSource Source { get; set; }

        public bool Pressed { get; protected set; }

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

        public bool SetPressed(bool pressed, [MaybeNullWhen(false)] out IMessage message)
        {
            this.Pressed = pressed;

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
