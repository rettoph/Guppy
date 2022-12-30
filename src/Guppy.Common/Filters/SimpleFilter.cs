using Guppy.Common.DependencyInjection.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Filters
{
    public abstract class SimpleFilter : IServiceFilter
    {
        public readonly Type Type;

        protected SimpleFilter(Type type)
        {
            this.Type = type;
        }

        public virtual void Initialize(IServiceProvider provier)
        {
            //
        }

        public virtual bool AppliesTo(IServiceConfiguration service)
        {
            var result = this.Type.IsAssignableFrom(service.Type);

            return result;
        }

        public abstract bool Invoke(IServiceProvider provider, IServiceConfiguration service);
    }
}
