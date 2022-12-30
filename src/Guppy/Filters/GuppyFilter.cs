using Guppy.Common;
using Guppy.Common.DependencyInjection.Interfaces;
using Guppy.Common.Filters;
using Guppy.Common.Implementations;
using Guppy.Common.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Filters
{
    public class GuppyFilter : SimpleFilter
    {
        public readonly Type GuppType;

        public GuppyFilter(Type type, Type guppy) : base(type)
        {
            ThrowIf.Type.IsNotAssignableFrom<IGuppy>(guppy);

            this.GuppType = guppy;
        }

        public override bool Invoke(IServiceProvider provider, IServiceConfiguration service)
        {
            var guppy = provider.GetRequiredService<ServiceActivator<IGuppy>>();

            if (guppy.Type is null)
            {
                return false;
            }

            var result = this.GuppType.IsAssignableFrom(guppy.Type);

            return result;
        }
    }

    public class GuppyFilter<T, TGuppy> : GuppyFilter
        where TGuppy : IGuppy
    {
        public GuppyFilter() : base(typeof(T), typeof(TGuppy))
        {
        }
    }
}
