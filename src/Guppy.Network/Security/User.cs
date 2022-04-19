using Guppy.Network.Security.Enums;
using Guppy.Network.Security.Structs;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Security
{
    public class User : IDisposable
    {

        private Dictionary<String, Claim> _claims;
        private List<Room> _rooms;

        public readonly int Id;
        public readonly DateTime CreatedAt;

        public IEnumerable<Claim> Claims => _claims.Values;
        public IEnumerable<Room> Rooms => _rooms;
        public Boolean IsCurrentUser { get; internal set; }
        public DateTime UpdatedAt { get; private set; }
        public NetPeer? NetPeer { get; internal set; }
        public String this[string key] => this.Claims.First(c => c.Key == key).Value;

        public event OnEventDelegate<User>? OnDisposing;

        internal User(int id, IEnumerable<Claim> claims, NetPeer? peer)
        {
            this.Id = id;
            this.NetPeer = peer;

            _claims = claims.ToDictionary(c => c.Key);
            _rooms = new List<Room>();

            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }

        public void SetClaims(IEnumerable<Claim> claims)
        {
            foreach (Claim claim in claims)
            {
                _claims[claim.Key] = claim;
            }

            this.UpdatedAt = DateTime.Now;
        }

        public void SetClaim(String key, String value, ClaimType type)
        {
            _claims[key] = new Claim(key, value, type);

            this.UpdatedAt = DateTime.Now;
        }

        internal void AddToRoom(Room room)
        {
            _rooms.Add(room);

            this.UpdatedAt = DateTime.Now;
        }

        internal void RemoveFromRoom(Room room)
        {
            _rooms.Remove(room);

            this.UpdatedAt = DateTime.Now;
        }

        public void Dispose()
        {
            this.OnDisposing?.Invoke(this);

            _claims.Clear();
        }
    }
}
