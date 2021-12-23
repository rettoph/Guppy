using Guppy.Network.Messages;
using Guppy.Network.Security.Dtos;
using Guppy.Network.Security.Enums;
using Guppy.Network.Security.Structs;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Security
{
    public class User : IDisposable
    {
        #region Private Fields
        private Dictionary<String, Claim> _claims;
        private List<Room> _rooms;
        #endregion

        #region Public Fields
        public readonly Int32 Id;

        public readonly NetPeer NetPeer;

        public readonly DateTime CreatedAt;
        #endregion

        #region Public Properties
        public IEnumerable<Claim> Claims => _claims.Values;
        public IEnumerable<Room> Rooms => _rooms;
        public DateTime UpdatedAt { get; private set; }
        #endregion

        #region Public Properties
        public String this[String key] => this.Claims.First(c => c.Key == key).Value;
        #endregion

        #region Events
        public event OnEventDelegate<User> OnDisposing;
        #endregion

        #region Constructors
        internal User(Int32 id, NetManager manager, IEnumerable<Claim> claims)
        {
            this.Id = id;
            this.NetPeer = id == -1 ? default : manager.GetPeerById(id);
            _claims = claims.ToDictionaryByValue(c => c.Key);
            _rooms = new List<Room>();

            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }
        #endregion

        #region Network Methods
        public UserDto GetDto(ClaimType lowestClaimType)
        {
            return new UserDto()
            {
                Id = this.Id,
                Claims = this.Claims.Where(c => c.Type >= lowestClaimType)
            };
        }
        #endregion

        #region Helper Methods
        public void SetClaims(IEnumerable<Claim> claims)
        {
            foreach(Claim claim in claims)
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
        #endregion

        #region IDisposable Implementation
        public void Dispose()
        {
            this.OnDisposing?.Invoke(this);

            _claims.Clear();
        }
        #endregion
    }
}
