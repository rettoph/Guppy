using Guppy.Core.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame
{
    internal class MonoGameTerminalSegment
    {
        public readonly Vector4 Color;
        public readonly string Text;

        public MonoGameTerminalSegment(IRef<Color> color, string text)
        {
            this.Color = color.GetValueOrFallback(Microsoft.Xna.Framework.Color.White).ToVector4();
            this.Text = text;
        }
    }
}
