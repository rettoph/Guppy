using Guppy.Common;
using Microsoft.Xna.Framework;
using Serilog.Events;

namespace Guppy.Game.Common
{
    public interface ITerminalTheme
    {
        IRef<Color> Get(LogEventLevel level);
    }
}
