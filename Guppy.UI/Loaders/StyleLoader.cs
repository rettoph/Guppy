using Guppy.Loaders;
using Guppy.UI.Styles;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Loaders
{
    public class StyleLoader : Loader<Type, Style, Style>
    {
        public StyleLoader(ILogger logger) : base(logger)
        {
        }
    }
}
