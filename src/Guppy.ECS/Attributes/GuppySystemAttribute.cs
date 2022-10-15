using Guppy.Attributes;
using Guppy.Common;
using Guppy.ECS.Definitions;
using Guppy.ECS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Attributes
{
    public sealed class GuppySystemAttribute : SystemFilterAttribute
    {
        private GuppySystemFilter _filter;

        public GuppySystemAttribute(Type guppy) : base()
        {
            _filter = new GuppySystemFilter(guppy);
        }

        protected override object GetInstance(Type classType, Type returnType)
        {
            return _filter;
        }
    }
}
