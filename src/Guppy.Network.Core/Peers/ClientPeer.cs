using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Messages;
using Guppy.Network.Enums;
using Guppy.Network.Security.Structs;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Threading.Interfaces;
using Guppy.Network.Security;
using System.Threading.Tasks;
using LiteNetLib.Utils;

namespace Guppy.Network
{
    public class ClientPeer : Peer, IMessageProcessor<UserRoomActionMessage>
    {
        protected override void PreCreate(ServiceProvider provider)
        {
            base.PreCreate(provider);

            provider.Settings.Set(NetworkAuthorization.Slave);
        }

        #region Message Processors
        void IMessageProcessor<UserRoomActionMessage>.Process(UserRoomActionMessage message)
        {
            Room room = this.Rooms.GetById(message.RoomId);
            User user = this.Users.UpdateOrCreate(message.User);

            switch (message.Action)
            {
                case UserRoomAction.Joined:
                    room.Users.TryAdd(user);
                    break;
                case UserRoomAction.Left:
                    room.Users.TryRemove(user);
                    break;
            }
        }
        #endregion

        #region Start Method 
        /// <summary>
        /// Start logic thread and listening on available port
        /// </summary>
        public Task TryStart(Int32 period = 16)
        {
            this.manager.Start();

            return base.TryStartAsync(period);
        }
        #endregion

        #region Connect Methods
        public void Connect(String target, Int32 port, params Claim[] claims)
        {
            this.network.WriteMessage(
                this.room, 
                new ConnectionRequestMessage()
                {
                    NetworkProviderConfigurationHash = this.network.GetConfigurationHash(),
                    Claims = claims
                },
                out NetDataWriter writer,
                out _);

            this.manager.Connect(target, port, writer);
        }
        #endregion
    }
}
