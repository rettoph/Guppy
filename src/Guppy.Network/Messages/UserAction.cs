using Guppy.Network.Enums;
using Guppy.Network.Identity.Claims;

namespace Guppy.Network.Messages
{
    public class UserAction
    {
        public int Id { get; init; }

        public UserActionTypes Type { get; init; }

        public Claim[] Claims { get; init; } = Array.Empty<Claim>();

        public override bool Equals(object? obj)
        {
            return obj is UserAction action &&
                   Id == action.Id &&
                   Type == action.Type &&
                   EqualityComparer<Claim[]>.Default.Equals(Claims, action.Claims);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Type, Claims);
        }
    }
}
