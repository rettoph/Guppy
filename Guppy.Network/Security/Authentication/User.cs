using Guppy.Enums;
using Guppy.Implementations;
using Guppy.Network.Security.Enums;
using Guppy.Utilities.Delegaters;
using Guppy.Utilities.Pools;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Security.Authentication
{
    public class User : NetworkEntity
    {
        #region Private Fields
        private IServiceProvider _provider;
        private Pool<Claim> _claimPool;
        private Dictionary<String, Claim> _claims;
        #endregion

        #region Public Attributes
        public NetConnection NetConnection { get; internal set; }
        #endregion


        #region Constructor
        public User(IServiceProvider provider, Pool<Claim> claimPool)
        {
            _provider = provider;
            _claimPool = claimPool;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            // Create a new claim
            _claims = new Dictionary<String, Claim>();
        }

        public override void Dispose()
        {
            base.Dispose();

            // Put all the old user claims back into the pool
            foreach (KeyValuePair<String, Claim> kvp in _claims)
                _claimPool.Put(kvp.Value);
            // Clear the current claims
            _claims.Clear();
        }
        #endregion

        #region Claim Methods
        public void AddClaim(String key, String value, ClaimScope scope = ClaimScope.Public)
        {
            if(_claims.ContainsKey(key))
            {
                _claims[key].Value = value;
                _claims[key].Scope = scope;
            }
            else
            {
                _claims[key] = _claimPool.Pull(_provider, c =>
                {
                    c.Value = value;
                    c.Scope = scope;
                });
            }

        }

        public String GetClaim(String key)
        {
            return _claims[key]?.Value;
        }
        #endregion

        #region Network Methods
        protected override void Write(NetOutgoingMessage om)
        {
            base.Write(om);

            // Load all public claims...
            var claims = _claims.Where(c => c.Value.Scope != ClaimScope.Private);

            // Write the claim count
            om.Write(claims.Count());

            // Write all claim data
            foreach(KeyValuePair<String, Claim> kvp in claims)
            {
                om.Write(kvp.Key);
                om.Write(kvp.Value.Value);
            }
        }

        protected override void Read(NetIncomingMessage im)
        {
            base.Read(im);

            var claims = im.ReadInt32();

            // Read all recieved claims
            for(Int32 i=0; i<claims; i++)
                this.AddClaim(
                    im.ReadString(), 
                    im.ReadString(), 
                    ClaimScope.Public);
        }
        #endregion
    }
}
