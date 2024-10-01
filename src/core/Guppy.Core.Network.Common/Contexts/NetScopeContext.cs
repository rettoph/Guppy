using Autofac;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Peers;

namespace Guppy.Core.Network.Common.Contexts
{
    public class NetScopeContext<T>(PeerType peerType, byte groupId)
    {
        public readonly PeerType PeerType = peerType;
        public readonly byte GroupId = groupId;

        public TGroup GetGroup<TGroup>(ILifetimeScope scope)
            where TGroup : class, INetGroup
        {
            IPeer peer = this.PeerType switch
            {
                PeerType.Client => scope.Resolve<IClientPeer>(),
                PeerType.Server => scope.Resolve<IServerPeer>(),
                _ => throw new NotImplementedException()
            };

            INetGroup group = peer.Groups.GetById(this.GroupId);

            if (group is not TGroup casted)
            {
                throw new Exception();
            }

            return casted;
        }
    }
}
