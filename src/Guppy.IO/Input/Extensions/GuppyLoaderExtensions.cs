using Guppy.IO.Input.ServiceLoaders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Input.Extensions
{
    public static class GuppyLoaderExtensions
    {
        public static GuppyLoader ConfigureInput(this GuppyLoader guppy)
        {
            guppy.RegisterServiceLoader(new InputServiceLoader());

            return guppy;
        }
    }
}
