using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Guppy.Network.Security.Enums;
using Lidgren.Network;
using System.Linq;

namespace Guppy.Network.Security
{
    public class User : NetworkObject
    {
        private Dictionary<String, Claim> _claims;

        public String this[String key]
        {
            get { return this.Get(key); }
        }

        #region Constructors
        public User()
        {
            _claims = new Dictionary<String, Claim>();
        }
        public User(Guid id) : base(id)
        {
            _claims = new Dictionary<String, Claim>();
        }
        #endregion

        public String Get(String key)
        {
            return _claims[key].Value;
        }

        public void Set(String key, Claim value)
        {
            _claims[key] = value;
        }
        public void Set(String key, ClaimType type, String value)
        {
            this.Set(key, new Claim(type, value));
        }
        public void Set(String key, String value)
        {
            this.Set(key, ClaimType.Public, value);
        }

        public override void Read(NetIncomingMessage im)
        {
            base.Read(im);

            // Read any incoming claims
            Int32 claimsCount = im.ReadInt32();
            for (Int32 i = 0; i < claimsCount; i++)
            {
                ClaimType claimType = (ClaimType)im.ReadByte();
                if (claimType == ClaimType.Private)
                    throw new Exception("Private claim sent from peer!");
                this.Set(im.ReadString(), claimType, im.ReadString());
            }
        }

        public override void Write(NetOutgoingMessage om)
        {
            this.Write(om, ClaimType.Public);
        }

        public void Write(NetOutgoingMessage om, ClaimType type)
        {
            base.Write(om);

            // Write the total number of claims and all claim values
            var outboundClaims = _claims.Where(c => c.Value.Type <= type);

            om.Write(outboundClaims.Count());
            foreach (KeyValuePair<String, Claim> claim in outboundClaims)
            {
                om.Write(claim.Key);
                om.Write((Byte)claim.Value.Type);
                om.Write(claim.Value.Value);
            }
        }
    }
}
