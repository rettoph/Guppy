using Autofac;
using Guppy.Common;
using Guppy.Common.Extensions.Autofac;
using Guppy.Common.Filters;

namespace Guppy.Attributes
{
    public class GuppyFilterAttribute : GuppyConfigurationAttribute
    {
        public readonly Type GuppyType;

        public GuppyFilterAttribute(Type guppyType)
        {
            ThrowIf.Type.IsNotAssignableFrom<IGuppy>(guppyType);

            this.GuppyType = guppyType;
        }

        protected override void Configure(ContainerBuilder builder, Type classType)
        {
            builder.RegisterFilter(new ServiceFilter(classType, this.GuppyType));
        }
    }

    public class GuppyFilterAttribute<TGuppy> : GuppyFilterAttribute
        where TGuppy : IGuppy
    {
        public GuppyFilterAttribute() : base(typeof(TGuppy)) { }
    }
}
