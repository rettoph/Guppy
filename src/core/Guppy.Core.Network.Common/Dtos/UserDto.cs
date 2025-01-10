using Guppy.Core.Network.Common.Claims;

namespace Guppy.Core.Network.Common.Dtos
{
    public sealed class UserDto
    {
        public int Id { get; init; }

        public Claim[] Claims { get; init; } = [];
    }
}