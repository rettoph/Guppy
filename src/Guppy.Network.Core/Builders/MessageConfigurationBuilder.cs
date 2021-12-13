using Guppy.Network.Configurations;
using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using System;

namespace Guppy.Network.Builders
{
    public class MessageConfigurationBuilder
    {
        #region Private Fields
        private String _name;
        private Type _dataType;
        private Action<NetDataWriter, IData> _dataWriter;
        private Func<NetDataReader, IData> _dataReader;
        #endregion

        #region Public Properties
        public String Name
        {
            get => _name;
            set => this.SetName(value);
        }

        public Type DataType
        {
            get => _dataType;
            set => this.SetDataType(value);
        }

        public Action<NetDataWriter, IData> DataWriter
        {
            get => _dataWriter;
            set => this.SetDataWriter(value);
        }

        public Func<NetDataReader, IData> DataReader
        {
            get => _dataReader;
            set => this.SetDataReader(value);
        }

        public Int32 Priority { get; set; }
        #endregion

        #region SeName Methods
        public MessageConfigurationBuilder SetName(String name)
        {
            _name = name;

            return this;
        }
        #endregion

        #region SetType Methods
        public MessageConfigurationBuilder SetDataType(Type dataType)
        {
            if (typeof(IData).ValidateAssignableFrom(dataType))
            {
                _dataType = dataType;
            }

            return this;
        }
        #endregion

        #region SetWriter Methods
        public MessageConfigurationBuilder SetDataWriter(Action<NetDataWriter, IData> dataWriter)
        {
            _dataWriter = dataWriter;

            return this;
        }
        #endregion

        #region SetReader Methods
        public MessageConfigurationBuilder SetDataReader(Func<NetDataReader, IData> dataReader)
        {
            _dataReader = dataReader;

            return this;
        }
        #endregion

        #region Build Methods
        public TConfiguration Build(DynamicIdSize dynamicIdSize)
        {
            if (!this.Id.HasValue)
            {
                throw new InvalidOperationException($"{nameof(PacketConfigurationBuilder)}::{nameof(Build)} - Ensure {nameof(PacketConfigurationBuilder)}.{nameof(Id)} has a value by calling {nameof(PacketConfigurationBuilder)}::{nameof(SetId)} at least once.");
            }

            UInt16 id = this.Id.Value;

            Byte[] idBytes = dynamicIdSize switch
            {
                DynamicIdSize.OneByte => new Byte[] { BitConverter.GetBytes(id)[0] },
                DynamicIdSize.TwoBytes => BitConverter.GetBytes(id),
                _ => throw new ArgumentOutOfRangeException(nameof(dynamicIdSize))
            };

            return this.Build(idBytes);
        }

        protected override MessageConfiguration Build(byte[] idBytes)
        {
            return new MessageConfiguration(
                this.Id.Value,
                idBytes,
                this.Name,
                this.Type,
                this.Writer,
                this.Reader);
        }
        #endregion
    }
}
