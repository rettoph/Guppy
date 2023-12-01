using Autofac;
using Guppy.Attributes;
using Guppy.Common.Autofac;
using Guppy.Extensions.Autofac;
using Guppy.Network.Enums;

namespace Guppy.Network
{
    [AutoLoad]
    internal class PeerTypeState : State<PeerType>
    {
        private readonly NetScope? _scope;

        public PeerTypeState(ILifetimeScope scope)
        {
            if(scope.HasTag(LifetimeScopeTags.GuppyScope))
            {
                scope.TryResolve(out _scope);
            }
        }

        public override PeerType GetValue()
        {
            return _scope?.Peer?.Type ?? PeerType.None;
        }

        public override bool Matches(PeerType value)
        {
            return value.HasFlag(this.GetValue());
        }
    }
}
