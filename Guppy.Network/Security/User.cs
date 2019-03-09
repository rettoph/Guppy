using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Lidgren.Network;

namespace Guppy.Network.Security
{
    public class User : NetworkObject
    {
        private Dictionary<String, String> _claims;

        public String this[String key]
        {
            get { return this.Get(key); }
        }

        #region Constructors
        public User()
        {
            _claims = new Dictionary<String, String>();
        }
        public User(Guid id) : base(id)
        {
            _claims = new Dictionary<String, String>();
        }
        #endregion

        public String Get(String key)
        {
            return _claims[key];
        }

        public void Set(String key, String value)
        {
            _claims[key] = value;
        }

        public override void Read(NetIncomingMessage im)
        {
            base.Read(im);

            // Read any incoming claims
            Int32 claimsCount = im.ReadInt32();
            for (Int32 i = 0; i < claimsCount; i++)
                _claims[im.ReadString()] = im.ReadString();
        }

        public override void Write(NetOutgoingMessage om)
        {
            base.Write(om);

            // Write the total number of claims and all claim values
            om.Write(_claims.Count);
            foreach (KeyValuePair<String, String> claim in _claims)
            {
                om.Write(claim.Key);
                om.Write(claim.Value);
            }

        }
    }
}
