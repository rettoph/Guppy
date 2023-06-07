using Guppy.Attributes;
using Guppy.Common.Implementations;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly ServiceActivator<IGuppy> _guppy;

        public GuppyTypeState(ServiceActivator<IGuppy> guppy)
        {
            _guppy = guppy;
        }

        public override Type? GetValue()
        {
            return _guppy.Type;
        }

        public override bool Matches(Type? value)
        {
            if(_guppy.Type is null)
            {
                return false;
            }

            return value?.IsAssignableFrom(_guppy.Type) ?? false;
        }
    }
}
