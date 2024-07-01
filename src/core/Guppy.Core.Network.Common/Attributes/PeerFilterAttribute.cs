using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Filters;

namespace Guppy.Core.Network.Common.Attributes
{
    public class PeerFilterAttribute : GuppyConfigurationAttribute
    {
        public readonly PeerType RequiredPeerType;

        public PeerFilterAttribute(PeerType requiredPeerType)
        {
            this.RequiredPeerType = requiredPeerType;
        }

        protected override void Configure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            builder.RegisterFilter(new StateServiceFilter<PeerType>(classType, StateKey<PeerType>.Default, this.RequiredPeerType));
        }
    }
}
