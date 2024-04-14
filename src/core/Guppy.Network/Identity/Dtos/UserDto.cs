using Guppy.Network.Identity.Claims;

namespace Guppy.Network.Identity.Dtos
{
    public sealed class UserDto
    {
        public int Id { get; init; }

        public Claim[] Claims { get; init; } = Array.Empty<Claim>();
    }
}
