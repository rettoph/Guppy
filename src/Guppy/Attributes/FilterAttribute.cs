using Guppy.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Attributes
{
    public abstract class FilterAttribute : Attribute
    {
        public FilterAttribute()
        {
        }

        public virtual void Initialize(IServiceCollection services, Type classType)
        {

        }
    }
}
