using Guppy.Enums;
using Guppy.Implementations;
using Guppy.Network.Security.Enums;
using Guppy.Utilities.Delegaters;
using Guppy.Utilities.Pools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Security.Authentication
{
    public class User : Entity
    {
        #region Private Fields
        private IServiceProvider _provider;
        private Pool<Claim> _claimPool;
        private Dictionary<String, Claim> _claims;
        #endregion

        #region Constructor
        public User(IServiceProvider provider, Pool<Claim> claimPool)
        {
            _provider = provider;
            _claimPool = claimPool;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize()
        {
            base.Initialize();

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
            _claims[key] = _claimPool.Pull(_provider, c =>
            {
                c.Value = value;
                c.Scope = scope;
            });
        }

        public String GetClaim(String key)
        {
            return _claims[key]?.Value;
        }
        #endregion

        #region Network Methods
        #endregion
    }
}
