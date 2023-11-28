using Autofac;
using Guppy.Attributes;
using Guppy.Common.Autofac;
using Guppy.Network.Enums;

namespace Guppy.Network
{
    [AutoLoad]
    internal class PeerTypeState : State<PeerType>
    {
        private readonly NetScope? _scope;

        public PeerTypeState(ILifetimeScope scope)
        {
            try
            {
                scope.TryResolve(out _scope);
            }
            catch
            {

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
