﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Configuration
{
    public class ResourcePackConfiguration
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public Dictionary<string, string[]> Import { get; init; } = new Dictionary<string, string[]>();
    }
}