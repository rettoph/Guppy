using Guppy.Core.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame
{
    internal class MonoGameTerminalSegment(IRef<Color> color, string text)
    {
        public readonly Vector4 Color = color.Value.ToVector4();
        public readonly string Text = text;
    }
}