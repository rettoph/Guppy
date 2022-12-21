using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class InitializableAttribute : Attribute
    {
        public virtual void Initialize(IServiceCollection services, Type classType)
        {

        }
    }
}
