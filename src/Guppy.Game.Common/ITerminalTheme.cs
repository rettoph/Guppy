using Guppy.Common;
using Microsoft.Xna.Framework;
using Serilog.Events;

namespace Guppy.Game.Common
{
    public interface ITerminalTheme
    {
        Ref<Color> Default { get; }
        Ref<Color> Fatal { get; }
        Ref<Color> Error { get; }
        Ref<Color> Warning { get; }
        Ref<Color> Information { get; }
        Ref<Color> Debug { get; }
        Ref<Color> Verbose { get; }

        Color Get(LogEventLevel level);
    }
}
