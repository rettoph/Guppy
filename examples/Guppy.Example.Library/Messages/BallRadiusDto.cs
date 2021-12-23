using Guppy.Network;
using Guppy.Network.Interfaces;
using Guppy.Threading.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Messages
{
    public sealed class BallRadiusDto : IPacket
    {
        public readonly Single Radius;

        public BallRadiusDto(float radius)
        {
            this.Radius = radius;
        }


        #region Read/Write Methods
        internal static BallRadiusDto Read(NetDataReader reader, NetworkProvider network)
        {
            return new BallRadiusDto(reader.GetFloat());
        }

        internal static void Write(NetDataWriter writer, NetworkProvider network, BallRadiusDto dto)
        {
            writer.Put(dto.Radius);
        }
        #endregion

        #region IMessage Implementation
        void IData.Clean()
        {
            // throw new NotImplementedException();
        }
        #endregion
    }
}
