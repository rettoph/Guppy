using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Attributes;
using Microsoft.Extensions.Logging;

namespace Guppy.Loaders
{
    [IsLoaderAttribute(90)]
    public class StringLoader : SimpleLoader<String, String>
    {
        public StringLoader(ILogger logger) : base(logger)
        {
        }
    }
}
