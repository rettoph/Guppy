using DotNetUtils.General.Interfaces;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Builders
{
    public abstract class DataConfigurationBuilder<TConfigurationBuilder, TConfiguration> : DynamicIdConfigurationBuilder<TConfigurationBuilder>, IPrioritizable<TConfigurationBuilder>
        where TConfigurationBuilder : DataConfigurationBuilder<TConfigurationBuilder, TConfiguration>
    {
        #region Public Properties
        public Type DataType { get; private set; }

        public Action<NetDataWriter, IData> DataWriter { get; private set; }

        public Func<NetDataReader, IData> DataReader { get; private set; }

        public Int32 Priority { get; set; }
        #endregion

        #region SetType Methods
        public TConfigurationBuilder SetDataType(Type type)
        {
            if (typeof(IData).ValidateAssignableFrom(type))
            {
                this.DataType = type;
            }

            return this as TConfigurationBuilder;
        }
        #endregion

        #region SetWriter Methods
        public TConfigurationBuilder SetDataWriter(Action<NetDataWriter, IData> writer)
        {
            this.DataWriter = writer;

            return this as TConfigurationBuilder;
        }
        #endregion

        #region SetReader Methods
        public TConfigurationBuilder SetDataReader(Func<NetDataReader, IData> reader)
        {
            this.DataReader = reader;

            return this as TConfigurationBuilder;
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

        protected abstract TConfiguration Build(Byte[] idBytes);
        #endregion
    }
}
