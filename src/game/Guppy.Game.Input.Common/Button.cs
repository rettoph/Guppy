using System.Diagnostics.CodeAnalysis;

namespace Guppy.Game.Input.Common
{
    internal sealed class Button<T>(
        string key,
        ButtonSource defaultSource,
        (bool pressed, T data)[] data) : IButton
        where T : IInput
    {
        public string Key { get; } = key;

        public ButtonSource DefaultSource { get; } = defaultSource;

        public ButtonSource Source { get; set; } = defaultSource;

        public bool Pressed { get; private set; }

        public readonly IReadOnlyDictionary<bool, T> Data = data.ToDictionary(x => x.pressed, x => x.data);

        public bool SetPressed(bool pressed, [MaybeNullWhen(false)] out IInput message)
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