using Guppy.IO.ServiceLoaders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Extensions
{
    public static class GuppyLoaderExtensions
    {
        public static GuppyLoader ConfigureTerminal(this GuppyLoader guppy)
        {
            guppy.RegisterServiceLoader(new TerminalServiceLoader());

            return guppy;
        }
    }
}
