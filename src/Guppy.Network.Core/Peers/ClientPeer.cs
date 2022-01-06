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
using Guppy.Network.Security.Enums;
using LiteNetLib;

namespace Guppy.Network
{
    public class ClientPeer : Peer, IDataProcessor<UserRoomActionMessage>
    {
        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Settings.Set(NetworkAuthorization.Slave);
        }
        #endregion

        #region Message Processors
        Boolean IDataProcessor<UserRoomActionMessage>.Process(UserRoomActionMessage message)
        {
            Room room = this.Rooms.GetById(message.RoomId);
            User user = this.Users.UpdateOrCreate(message.User);

            switch (message.Action)
            {
                case UserRoomAction.Joined:
                    return room.Users.TryAdd(user);
                case UserRoomAction.Left:
                    return room.Users.TryRemove(user);
            }

            return false;
        }
        #endregion

        #region Start Method 
        /// <summary>
        /// Start logic thread and listening on available port
        /// </summary>
        public new Task TryStart(Int32 period = 16)
        {
            this.manager.Start();

            return base.TryStart(period);
        }
        #endregion

        #region Connect Methods
        public void TryConnect(String target, Int32 port, params Claim[] claims)
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
