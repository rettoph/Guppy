using Guppy.Common;

namespace Guppy
{
    internal class GuppyEnvironment : IGuppyEnvironment
    {
        public required string Company { get; init; }
        public required string Name { get; init; }
    }
}
