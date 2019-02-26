using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Loaders
{
    public class ColorLoader : Loader<String, Color, Color>
    {
        Color? test;

        public ColorLoader(ILogger logger) : base(logger)
        {
        }
    }
}
