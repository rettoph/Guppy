using Guppy.Attributes;
using Guppy.Network.Enums;

namespace Guppy.Network
{
    [AutoLoad]
    internal class PeerTypeState : State<PeerType>
    {
        private readonly NetScope _scope;

        public PeerTypeState(NetScope scope)
        {
            _scope = scope;
        }

        public override PeerType GetValue()
        {
            return _scope.Peer?.Type ?? PeerType.None;
        }

        public override bool Matches(PeerType value)
        {
            return value.HasFlag(this.GetValue());
        }
    }
}
