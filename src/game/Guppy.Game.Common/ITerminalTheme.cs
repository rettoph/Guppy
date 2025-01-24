using Guppy.Core.Common;
using Guppy.Core.Logging.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Common
{
    public interface ITerminalTheme
    {
        IRef<Color> Get(LogLevelEnum level);
    }
}