using Guppy.Attributes;
using Guppy.Common;
using Guppy.ECS.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Attributes
{
    public abstract class SystemFilterAttribute : FactoryAttribute
    {
        protected SystemFilterAttribute() : base(typeof(IFilter<ISystemDefinition>))
        {
        }
    }
}
