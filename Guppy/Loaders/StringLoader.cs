﻿using Guppy.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Loaders
{
    [IsLoader]
    public class StringLoader : Loader<String, String, String>
    {
        public StringLoader(ILogger logger) : base(logger)
        {
        }

        public void TryRegister(String handle, String value, UInt16 priority = 100)
        {
            this.logger.LogDebug($"Registering new String<{handle}>({priority}) => '{value}'");

            base.Register(handle, value, priority);
        }
    }
}
