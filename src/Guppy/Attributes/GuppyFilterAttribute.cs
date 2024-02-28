using Autofac;
using Guppy.Common;
using Guppy.Common.Extensions.Autofac;
using Guppy.StateMachine;
using Guppy.StateMachine.Filters;

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
            builder.RegisterFilter(new StateServiceFilter<Type>(classType, new State<Type>(
                key: StateKey<Type>.Create<IGuppy>(),
                value: this.GuppyType)));
        }
    }

    public class GuppyFilterAttribute<TGuppy> : GuppyFilterAttribute
        where TGuppy : IGuppy
    {
        public GuppyFilterAttribute() : base(typeof(TGuppy)) { }
    }
}
