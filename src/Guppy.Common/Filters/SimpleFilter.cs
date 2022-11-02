using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Filters
{
    public abstract class SimpleFilter : IFilter
    {
        public readonly Type ImplementationType;

        protected SimpleFilter(Type implementationType)
        {
            this.ImplementationType = implementationType;
        }

        public virtual void Initialize(IServiceProvider provider)
        {

        }

        public virtual bool AppliesTo(Type implementationType)
        {
            var result = implementationType == this.ImplementationType;

            return result;
        }

        public abstract bool Invoke(IServiceProvider provider, Type implementationType);
    }
}
