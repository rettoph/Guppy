using Guppy.Common;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    public class TerminalTheme
    {
        public Ref<Color> Default;
        public Ref<Color> Fatal;
        public Ref<Color> Error;
        public Ref<Color> Warning;
        public Ref<Color> Information;
        public Ref<Color> Debug;
        public Ref<Color> Verbose;

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
