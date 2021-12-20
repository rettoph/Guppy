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
    public class User
    {
        #region Private Fields
        private Dictionary<String, Claim> _claims;
        #endregion


        #region Public Fields
        public readonly Int32 Id;

        #endregion

        #region Public Properties
        public IEnumerable<Claim> Claims => _claims.Values;
        #endregion

        #region Public Properties
        public String this[String key] => this.Claims.First(c => c.Key == key).Value;
        #endregion

        #region Constructors
        internal User(Int32 id, IEnumerable<Claim> claims)
        {
            this.Id = id;
            _claims = claims.ToDictionaryByValue(c => c.Key);
        }
        #endregion

        #region Network Methods
        public UserDto GetMessage(ClaimType lowestClaimType)
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
        }

        public void SetClaim(String key, String value, ClaimType type)
        {
            _claims[key] = new Claim(key, value, type);
        }
        #endregion
    }
}
