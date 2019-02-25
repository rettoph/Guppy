using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Guppy.Loaders
{
    /// <summary>
    /// Custom loader used to manage
    /// string values
    /// </summary>
    public class StringLoader : Loader<String, String, String>
    {
        public StringLoader(ILogger logger) : base(logger)
        {
        }
    }
}
