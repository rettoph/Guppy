using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Guppy.Network.Security.Dtos;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Messages
{
    internal sealed class UserRoomActionMessage : IData
    {
        public UserRoomAction Action { get; internal init; }
        public Byte RoomId { get; internal init; }
        public UserDto User { get; internal init; }


        #region Read/Write Methods
        public static UserRoomActionMessage Read(NetDataReader reader, NetworkProvider network)
        {
            return new UserRoomActionMessage()
            {
                Action = reader.GetEnum<UserRoomAction>(),
                RoomId = reader.GetByte(),
                User = UserDto.Read(reader)
            };
        }

        public static void Write(NetDataWriter writer, NetworkProvider network, UserRoomActionMessage message)
        {
            writer.Put(message.Action);
            writer.Put(message.RoomId);
            UserDto.Write(writer, message.User);
        }
        #endregion

        #region IDisposable Implementation
        void IData.Clean()
        {
            // throw new NotImplementedException();
        }
        #endregion
    }
}
