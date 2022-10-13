using Guppy.Attributes;
using Guppy.ECS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Attributes
{
    public sealed class SingletonSystemAttribute : WithFilterAttribute
    {
        public SingletonSystemAttribute() : base(typeof(SingletonSystemFilter))
        {
        }
    }
}
