using Guppy.DependencyInjection;
using Guppy.Lists;
using Guppy.Lists.Interfaces;
using Guppy.Network.Interfaces;
using Guppy.Network.Security;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Lists
{
    /// <summary>
    /// A global list of all known users.
    /// </summary>
    public class UserList : BaseNetworkList<IUser>
    {
        #region Public Properties
        public IList<NetConnection> Connections { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.Connections = new List<NetConnection>();

            this.OnAdded += this.HandleUserAdded;
            this.OnRemoved += this.HandleUserRemoved;
        }

        protected override void Release()
        {
            base.Release();

            this.OnAdded -= this.HandleUserAdded;
            this.OnRemoved -= this.HandleUserRemoved;

            this.Connections.Clear();
            this.Connections = null;
        }


        #endregion

        #region Create Methods
        /// <summary>
        /// Get or create a new <see cref="IUser"/> with the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IUser GetOrCreate(Guid id, params Claim[] claims)
        {
            var user = this.FirstOrDefault(c => c.Id == id);
            user ??= this.Create<IUser>(this.provider, (channel, p, c) =>
            {
                channel.Id = id;
            });

            foreach (Claim claim in claims)
                user.SetClaim(claim);

            return user;
        }
        #endregion

        #region Event Handlers
        private void HandleUserAdded(IServiceList<IUser> sender, IUser user)
        {
            if (user.Connection != default && !this.Connections.Contains(user.Connection))
                this.Connections.Add(user.Connection);
        }

        private void HandleUserRemoved(IServiceList<IUser> sender, IUser user)
        {
            if (user.Connection != default && this.Connections.Contains(user.Connection))
                this.Connections.Remove(user.Connection);
        }
        #endregion
    }
}
