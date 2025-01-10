using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Network.Common.Claims;
using Guppy.Core.Network.Common.Identity.Enums;
using LiteNetLib;

namespace Guppy.Core.Network.Common
{
    public interface IUser : IEnumerable<Claim>
    {
        int Id { get; }
        NetPeer? NetPeer { get; }
        UserStateEnum State { get; }

        event OnChangedEventDelegate<IUser, UserStateEnum> OnStateChanged;


        public void Set<T>(string key, T value, ClaimAccessibilityEnum accessibility);

        public void Set(IEnumerable<Claim> claims);

        public T Get<T>(string key);

        public bool TryGet<T>(string key, [MaybeNullWhen(false)] out T? value);
    }
}