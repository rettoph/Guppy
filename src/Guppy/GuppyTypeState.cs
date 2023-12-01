using Autofac;
using Guppy.Attributes;
using Guppy.Common.Autofac;
using Guppy.Common.Implementations;
using Guppy.Extensions.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    [AutoLoad]
    internal sealed class GuppyTypeState : State<Type?>
    {
        private readonly IGuppy? _guppy;

        public GuppyTypeState(ILifetimeScope scope)
        {
            if(scope.HasTag(LifetimeScopeTags.GuppyScope))
            {
                scope.TryResolve(out _guppy);
            }
        }

        public override Type? GetValue()
        {
            return _guppy?.GetType();
        }

        public override bool Matches(Type? value)
        {
            if(_guppy?.GetType() is null)
            {
                return false;
            }

            return value?.IsAssignableFrom(_guppy.GetType()) ?? false;
        }
    }
}
