using Guppy.IO.Input.ServiceLoaders;
using Guppy.IO.Output.ServiceLoaders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions
{
    public static class GuppyLoaderExtensions
    {
        public static GuppyLoader ConfigureTerminal(this GuppyLoader loader, String defaultTerminalFontPath)
        {
            loader.RegisterServiceLoader(new TerminalServiceLoader(defaultTerminalFontPath));

            return loader;
        }

        public static GuppyLoader ConfigureInput(this GuppyLoader guppy)
        {
            guppy.RegisterServiceLoader(new InputServiceLoader());

            return guppy;
        }
    }
}
