using Guppy.Attributes;
using Guppy.Common;
using Guppy.Enums;

namespace Guppy
{
    [Service<IState>(ServiceLifetime.Scoped, true)]
    public abstract class State : IState
    {
        public abstract bool Matches(object? value);
    }
}
