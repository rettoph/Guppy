using Autofac;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Peers;

namespace Guppy.Core.Network.Common.Contexts
{
    public class NetScopeContext<T>
    {
        public readonly PeerType PeerType;
        public readonly byte GroupId;

        public NetScopeContext(PeerType peerType, byte groupId)
        {
            PeerType = peerType;
            GroupId = groupId;
        }

        public T GetGroup<T>(ILifetimeScope scope)
            where T : class, INetGroup
        {
            IPeer peer = this.PeerType switch
            {
                PeerType.Client => scope.Resolve<IClientPeer>(),
                PeerType.Server => scope.Resolve<IServerPeer>(),
                _ => throw new NotImplementedException()
            };

            INetGroup group = peer.Groups.GetById(this.GroupId);

            if (group is not T casted)
            {
                throw new Exception();
            }

            return casted;
        }
    }
}
