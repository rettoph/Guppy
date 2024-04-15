using Autofac;
using Guppy.Engine.Attributes;
using Guppy.Engine.Common.Extensions.Autofac;
using Guppy.Core.Network.Enums;
using Guppy.Core.StateMachine;
using Guppy.Core.StateMachine.Filters;

namespace Guppy.Core.Network.Attributes
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
