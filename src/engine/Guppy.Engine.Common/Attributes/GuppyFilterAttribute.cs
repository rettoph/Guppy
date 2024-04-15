using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.StateMachine;
using Guppy.Core.StateMachine.Filters;
using Guppy.Engine.Common;

namespace Guppy.Core.Common.Attributes
{
    public class GuppyFilterAttribute : GuppyConfigurationAttribute
    {
        public readonly Type GuppyType;

        public GuppyFilterAttribute(Type guppyType)
        {
            ThrowIf.Type.IsNotAssignableFrom<IGuppy>(guppyType);

            this.GuppyType = guppyType;
        }

        protected override void Configure(IContainer boot, ContainerBuilder builder, Type classType)
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
