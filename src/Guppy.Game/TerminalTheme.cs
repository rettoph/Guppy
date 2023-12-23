using Guppy.Common;
using Guppy.Game.Common;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;
using Serilog.Events;

namespace Guppy.Game
{
    public class TerminalTheme : ITerminalTheme
    {
        public Ref<Color> Default { get; }
        public Ref<Color> Fatal { get; }
        public Ref<Color> Error { get; }
        public Ref<Color> Warning { get; }
        public Ref<Color> Information { get; }
        public Ref<Color> Debug { get; }
        public Ref<Color> Verbose { get; }

        public TerminalTheme(IResourceProvider resources)
        {
            this.Default = resources.Get(Resources.Colors.TerminalDefault);
            this.Fatal = resources.Get(Resources.Colors.TerminalFatal);
            this.Error = resources.Get(Resources.Colors.TerminalError);
            this.Warning = resources.Get(Resources.Colors.TerminalWarning);
            this.Information = resources.Get(Resources.Colors.TerminalInformation);
            this.Debug = resources.Get(Resources.Colors.TerminalDebug);
            this.Verbose = resources.Get(Resources.Colors.TerminalVerbose);
        }

        public Color Get(LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Verbose:
                    return this.Verbose;

                case LogEventLevel.Debug:
                    return this.Debug;

                case LogEventLevel.Information:
                    return this.Information;

                case LogEventLevel.Warning:
                    return this.Warning;

                case LogEventLevel.Error:
                    return this.Error;

                case LogEventLevel.Fatal:
                    return this.Fatal;

                default:
                    return this.Default;
            }
        }
    }
}
