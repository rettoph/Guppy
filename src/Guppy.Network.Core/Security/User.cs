using Guppy.DependencyInjection;
using Guppy.Enums;
using Guppy.Interfaces;
using Guppy.Network.Interfaces;
using Guppy.Network.Peers;
using Guppy.Network.Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Contexts;

namespace Guppy.Network.Security
{
    public class User : Service, IUser
    {
        #region Private Fields
        private Dictionary<String, Claim> _claims;
        #endregion

        #region Public Properties
        public NetConnection Connection { get; internal set; }
        #endregion

        #region IUser Implementation
        NetConnection IUser.Connection
        {
            get => this.Connection;
            set => this.Connection = value;
        }

        public String this[String claim] => this.Claims[claim].Value;

        public IReadOnlyDictionary<String, Claim> Claims => _claims;
        #endregion

        #region Lifecycle Methods
        protected override void Create(GuppyServiceProvider provider)
        {
            base.Create(provider);

            _claims = new Dictionary<String, Claim>();
        }

        protected override void Release()
        {
            base.Release();

            _claims.Clear();
        }

        protected override void Dispose()
        {
            base.Dispose();

            _claims = null;
        }
        #endregion

        #region Helper Methods
        public void AddClaim(Claim claim)
            => _claims.Add(claim.Key, claim);

        public void SetClaim(Claim claim)
            => _claims[claim.Key] = claim;
        #endregion
    }
}
