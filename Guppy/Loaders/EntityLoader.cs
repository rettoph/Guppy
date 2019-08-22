using Guppy.Attributes;
using Guppy.Utilities.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Loaders
{
    [IsLoader(110)]
    public class EntityLoader : Loader<String, EntityOptions, EntityOptions>
    {
        public EntityLoader(StringLoader stringLoader, ILogger logger) : base(logger)
        {
        }
    }
}
