using Autofac;
using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Extensions.Autofac;
using Guppy.Common.Filters;
using Guppy.Network.Enums;
using Guppy.Network.Peers;

namespace Guppy.Network.Attributes
{
    public class PeerFilterAttribute : GuppyConfigurationAttribute
    {
        public readonly object State;

        public PeerFilterAttribute(PeerType flags)
        {
            this.State = flags;
        }
        protected internal PeerFilterAttribute(Type type)
        {
            ThrowIf.Type.IsNotAssignableFrom<Peer>(type);

            this.State = type;
        }

        protected override void Configure(ContainerBuilder builder, Type classType)
        {
            builder.RegisterFilter(new ServiceFilter(classType, this.State));
        }
    }

    public class PeerTypeFilterAttribute<TPeer> : PeerFilterAttribute
        where TPeer : Peer
    {
        public PeerTypeFilterAttribute() : base(typeof(TPeer))
        {

        }
    }
}
