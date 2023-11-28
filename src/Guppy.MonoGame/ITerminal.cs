using Microsoft.Xna.Framework;

namespace Guppy.MonoGame
{
    public interface ITerminal
    {
        void WriteLine(string value);

        void WriteLine(string value, Color color);
    }
}
