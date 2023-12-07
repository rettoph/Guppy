using Autofac;
using Guppy.Attributes;
using Guppy.Common.Autofac;
using Guppy.Extensions.Autofac;
using Guppy.Network.Enums;

namespace Guppy.Network
{
    [AutoLoad]
    internal class PeerState : State
    {
        private readonly NetScope? _scope;

        public PeerState(ILifetimeScope scope)
        {
            if(scope.HasTag(LifetimeScopeTags.GuppyScope))
            {
                scope.TryResolve(out _scope);
            }
        }

        public override bool Matches(object? value)
        {
            if(_scope?.Peer is null)
            {
                return false;
            }

            if(value is Type peerType)
            {
                return _scope.Peer.GetType().IsAssignableTo(peerType);
            }

            if(value is PeerType peerTypeEnum)
            {
                return _scope.Peer.Type == peerTypeEnum;
            }

            return false;
        }
    }
}
