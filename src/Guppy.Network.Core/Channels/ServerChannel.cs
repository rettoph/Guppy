﻿using Guppy.DependencyInjection;
using Guppy.Lists.Interfaces;
using Guppy.Network.Extensions.Security;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Channels
{
    public class ServerChannel : Channel
    {
        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.Users.OnAdded += this.HandleUserJoined;
            this.Users.OnRemoved += this.HandleUserLeft;
        }
        #endregion

        #region Event Handlers
        private void HandleUserJoined(IServiceList<IUser> sender, IUser newUser)
        {
            // Broadcast a list of all users to the new user...
            foreach(IUser user in this.Users)
            {
                if(user != newUser)
                    this.Messages[GuppyNetworkCoreConstants.Messages.Channel.UserJoined].Create(om =>
                    {
                        user.TryWrite(om);
                    }, newUser.Connection);
            }

            // Broadcast the new users to all connected users.
            this.Messages[GuppyNetworkCoreConstants.Messages.Channel.UserJoined].Create(om =>
            {
                newUser.TryWrite(om);
            });
        }

        private void HandleUserLeft(IServiceList<IUser> sender, IUser oldUser)
        {
            // Broadcast the disconnected user id to all connected peers...
            this.Messages[GuppyNetworkCoreConstants.Messages.Channel.UserLeft].Create(om =>
            {
                om.Write(oldUser.Id);
            });
        }
        #endregion
    }
}
