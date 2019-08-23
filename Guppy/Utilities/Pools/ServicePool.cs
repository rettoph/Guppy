using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Pools
{
    public class ServicePool : Pool
    {
        public ServicePool(Type targetType) : base(targetType)
        {
        }

        protected override object Build(IServiceProvider provider)
        {
            return ActivatorUtilities.CreateInstance(provider, this.TargetType);
        }
    }
}
