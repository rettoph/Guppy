using System.Diagnostics.CodeAnalysis;

namespace Guppy.Game.Input
{
    public interface IButton
    {
        string Key { get; }
        ButtonSource DefaultSource { get; }
        ButtonSource Source { get; set; }
        bool Pressed { get; }

        bool SetPressed(bool pressed, [MaybeNullWhen(false)] out IInput message);
    }
}
