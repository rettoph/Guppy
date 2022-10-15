using Guppy.Attributes;
using Guppy.ECS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Attributes
{
    public sealed class SingletonSystemAttribute : SystemFilterAttribute
    {
        protected override object GetInstance(Type classType, Type returnType)
        {
            return SingletonSystemFilter.Instance;
        }
    }
}
