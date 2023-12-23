using Autofac;
using Guppy.Attributes;
using Guppy.Common.Autofac;
using Guppy.Extensions.Autofac;

namespace Guppy
{
    [AutoLoad]
    internal sealed class GuppyState : State
    {
        private readonly IGuppy? _guppy;

        public GuppyState(ILifetimeScope scope)
        {
            if (scope.HasTag(LifetimeScopeTags.GuppyScope))
            {
                scope.TryResolve(out _guppy);
            }
        }

        public override bool Matches(object? value)
        {
            if (_guppy?.GetType() is null)
            {
                return false;
            }

            if (value is Type guppyType)
            {
                return _guppy.GetType().IsAssignableTo(guppyType);
            }

            return false;
        }
    }
}
