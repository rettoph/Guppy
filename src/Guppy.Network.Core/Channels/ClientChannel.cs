using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Extensions.Security;
using Guppy.Network.Lists;
using Guppy.Network.Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Channels
{
    public class ClientChannel : Channel
    {
        #region Private Fields
        /// <summary>
        /// A list of all users.
        /// </summary>
        private UserList _allUsers;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _allUsers);
        }

        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);

            this.Messages[Constants.Messages.Channel.UserJoined].OnRead += this.ReadUserJoinedMessage;
            this.Messages[Constants.Messages.Channel.UserLeft].OnRead += this.ReadUserLeftMessage;
        }

        protected override void Release()
        {
            base.Release();

            this.Messages[Constants.Messages.Channel.UserJoined].OnRead -= this.ReadUserJoinedMessage;
            this.Messages[Constants.Messages.Channel.UserLeft].OnRead -= this.ReadUserLeftMessage;
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            _allUsers = null;
        }
        #endregion

        #region Message Handlers
        private void ReadUserJoinedMessage(MessageTypeManager sender, NetIncomingMessage im)
        {
            var user = _allUsers.GetOrCreate(im.ReadGuid());
            user.TryRead(im);

            this.Users.TryAdd(user);

            Console.WriteLine($"Added Users: {this.Users.Count}");
        }

        private void ReadUserLeftMessage(MessageTypeManager sender, NetIncomingMessage im)
        {
            var user = this.Users.GetById(im.ReadGuid());
            this.Users.TryRemove(user);

            Console.WriteLine($"Removed Users: {this.Users.Count}");
        }
        #endregion
    }
}
