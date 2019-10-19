using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.Network.Configurations;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Guppy.Pooling.Interfaces;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Utilitites.Delegaters;
using Microsoft.Xna.Framework;

namespace Guppy.Network.Groups
{
    /// <summary>
    /// Groups represent collections of connections that
    /// are capable of sending messages back and fourth
    /// to the connected peer.
    /// </summary>
    public abstract class Group : Frameable
    {
        #region Private Fields
        private Peer _peer;
        #endregion

        #region Internal Attributes
        protected internal abstract IList<NetConnection> connections { get; protected set; }
        #endregion

        #region Public Attributes
        public CreatableCollection<User> Users { get; private set; }
        public GroupMessageDelegater Messages { get; private set; }
        #endregion

        #region Constructor
        public Group(CreatableCollection<User> users, Peer peer)
        {
            _peer = peer;

            this.Users = users;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.Messages = provider.GetRequiredService<GroupMessageDelegater>();
            this.Messages.Group = this;
        }

        public override void Dispose()
        {
            base.Dispose();

            this.connections.Clear();

            this.Users.Clear();
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Messages.ReadAll();
            this.Messages.SendAll();
        }
        #endregion
    }
}
