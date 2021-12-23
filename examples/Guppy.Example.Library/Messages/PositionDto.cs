using Guppy.Network;
using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Messages
{
    public sealed class PositionDto : IPacket
    {
        public readonly Vector2 Position;

        public PositionDto(Vector2 position)
        {
            this.Position = position;
        }

        #region Read/Write Methods
        internal static PositionDto Read(NetDataReader reader, NetworkProvider network)
        {
            return new PositionDto(reader.GetVector2());
        }

        internal static void Write(NetDataWriter writer, NetworkProvider network, PositionDto dto)
        {
            writer.Put(dto.Position);
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
