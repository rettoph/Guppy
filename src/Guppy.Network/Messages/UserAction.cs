using Guppy.Network.Identity.Claims;

namespace Guppy.Network.Messages
{
    public class UserAction
    {
        public enum Actions
        {
            ConnectionRequest,
            Connected,
            CurrentUserConnected,
            UserJoined,
            UserLeft
        }

        public int Id { get; init; }

        public Actions Action { get; init; }

        public Claim[] Claims { get; init; } = Array.Empty<Claim>();

        public override bool Equals(object? obj)
        {
            return obj is UserAction action &&
                   Id == action.Id &&
                   Action == action.Action &&
                   EqualityComparer<Claim[]>.Default.Equals(Claims, action.Claims);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Action, Claims);
        }
    }
}
