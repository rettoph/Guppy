using Guppy.Common;
using Guppy.Common.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    internal class GuppyEnvironment : IGuppyEnvironment
    {
        public required string Company { get; init; }
        public required string Name { get; init; }
    }
}
