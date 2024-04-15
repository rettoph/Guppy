using Guppy.Core.Messaging.Common;

namespace Guppy.Game.MonoGame
{
    public sealed class MenuItem
    {
        public required string Label { get; init; }
        public required IMessage OnClick { get; init; }

        public MenuItem()
        {

        }
        public MenuItem(string label, IMessage onClick)
        {
            this.Label = label;
            this.OnClick = onClick;
        }
    }
}
