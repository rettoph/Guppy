using Autofac;
using Guppy.Attributes;
using Guppy.Common.Extensions.Autofac;
using Guppy.Network.Enums;
using Guppy.StateMachine;
using Guppy.StateMachine.Filters;

namespace Guppy.Network.Attributes
{
    public class PeerFilterAttribute : GuppyConfigurationAttribute
    {
        public readonly PeerType RequiredPeerType;

        public PeerFilterAttribute(PeerType requiredPeerType)
        {
            this.RequiredPeerType = requiredPeerType;
        }

        protected override void Configure(ContainerBuilder builder, Type classType)
        {
            builder.RegisterFilter(new StateServiceFilter<PeerType>(classType, new State<PeerType>(this.RequiredPeerType)));
        }
    }
}
