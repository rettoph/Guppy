using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Attributes;
using Guppy.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Attributes
{
    public class GuppyFilterAttribute : InitializableAttribute
    {
        public readonly Type GuppyType;

        public GuppyFilterAttribute(Type guppyType)
        {
            ThrowIf.Type.IsNotAssignableFrom<IGuppy>(guppyType);

            this.GuppyType = guppyType;
        }

        protected override void Initialize(IServiceCollection services, Type classType)
        {
            services.AddFilter(new GuppyFilter(classType, this.GuppyType));
        }
    }

    public class GuppyFilterAttribute<TGuppy> : GuppyFilterAttribute
    {
        public GuppyFilterAttribute() : base(typeof(TGuppy)) { }
    }
}
