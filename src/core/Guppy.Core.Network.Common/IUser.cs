using Guppy.Core.Network.Common.Claims;
using Guppy.Core.Network.Common.Identity.Enums;
using LiteNetLib;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Core.Network.Common
{
    public interface IUser : IEnumerable<Claim>
    {
        int Id { get; }
        NetPeer? NetPeer { get; }
        UserState State { get; }

        event OnChangedEventDelegate<IUser, UserState> OnStateChanged;


        public void Set<T>(string key, T value, ClaimAccessibility accessibility);

        public void Set(IEnumerable<Claim> claims);

        public T Get<T>(string key);

        public bool TryGet<T>(string key, [MaybeNullWhen(false)] out T? value);
    }
}
