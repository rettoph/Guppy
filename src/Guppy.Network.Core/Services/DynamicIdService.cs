using Guppy.Network.Configurations;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Services
{
    public class DynamicIdService<TDynamicId>
        where TDynamicId : DynamicIdConfiguration
    {
        #region Private Fields
        private Byte[] _buffer;
        private Func<NetDataReader, UInt16> _idReader;
        #endregion

        #region Public Properties
        /// <summary>
        /// Magic number indicating the maximum size in bytes a packet id is when sent through
        /// the network.
        /// </summary>
        public DynamicIdSize IdSize { get; private set; }
        #endregion

        #region Constructor
        internal DynamicIdService(DynamicIdSize idSize)
        {
            _buffer = new Byte[2];

            this.IdSize = idSize;

            _idReader = this.IdSize switch
            {
                DynamicIdSize.OneByte => this.ReadId_OneByte,
                DynamicIdSize.TwoBytes => this.ReadId_TwoBytes,
                _ => throw new ArgumentOutOfRangeException(nameof(this.IdSize))
            };
        }
        #endregion

        #region Read/Write Methods
        protected UInt16 ReadId(NetDataReader im)
            => _idReader(im);

        protected void WriteId(NetDataWriter om, TDynamicId item)
        {
            om.Put(item.IdBytes);
        }
        #endregion

        #region ReadPacketId Methods
        private UInt16 ReadId_OneByte(NetDataReader im)
        {
            _buffer[0] = im.GetByte();
            return BitConverter.ToUInt16(_buffer);
        }

        private UInt16 ReadId_TwoBytes(NetDataReader im)
        {
            return im.GetUShort();
        }
        #endregion
    }
}
