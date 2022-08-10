using Guppy.Network.Identity.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Messages
{
    public class UserAction
    {
        public enum Actions
        {
            ConnectionRequest,
            Connected,
            CurrentUserConnected
        }

        public int Id { get; init; }

        public Actions Action { get; init; }

        public Claim[] Claims { get; init; } = Array.Empty<Claim>();
    }
}
