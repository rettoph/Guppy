using Guppy.Common;

namespace Guppy
{
    public class GuppyEnvironment : IGuppyEnvironment
    {
        public required string Company { get; init; }
        public required string Name { get; init; }
    }
}
