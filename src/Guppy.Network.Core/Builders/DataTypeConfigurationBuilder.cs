using Minnow.General.Interfaces;
using Guppy.Network.Configurations;
using Guppy.Network.Interfaces;
using Guppy.Network.Structs;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Builders
{
    public abstract class DataTypeConfigurationBuilder : IFluentPrioritizable<DataTypeConfigurationBuilder>
    {
        #region Protected Properties
        protected NetworkProviderBuilder network { get; private set; }
        #endregion

        #region Public Properties
        public Type Type { get; }

        public Int32 Priority { get; set; }
        #endregion

        #region Constructor
        internal DataTypeConfigurationBuilder(Type type, NetworkProviderBuilder network)
        {
            this.network = network;

            this.Type = type;
        }
        #endregion

        #region Build Methods
        public abstract DataTypeConfiguration Build(DynamicId id);
        #endregion
    }

    public class DataTypeConfigurationBuilder<TData> : DataTypeConfigurationBuilder
        where TData : class, IData
    {
        #region Private Fields
        private Action<NetDataWriter, TData> _writer;
        private Func<NetDataReader, TData> _reader;
        #endregion

        #region Public Properties
        public Action<NetDataWriter, TData> Writer
        {
            get => _writer;
            set => this.SetWriter(value);
        }

        public Func<NetDataReader, TData> Reader
        {
            get => _reader;
            set => this.SetReader(value);
        }
        #endregion

        #region Constructor
        internal DataTypeConfigurationBuilder(Type type, NetworkProviderBuilder network) : base(type, network)
        {
        }
        #endregion

        #region SetWriter Methods
        public DataTypeConfigurationBuilder<TData> SetWriter(Action<NetDataWriter, TData> dataWriter)
        {
            _writer = dataWriter;

            return this;
        }
        #endregion

        #region SetReader Methods
        public DataTypeConfigurationBuilder<TData> SetReader(Func<NetDataReader, TData> dataReader)
        {
            _reader = dataReader;

            return this;
        }
        #endregion

        #region RegisterMessage Methods
        public DataTypeConfigurationBuilder<TData> RegisterMessage(String name, Action<MessageConfigurationBuilder<TData>> builder)
        {
            MessageConfigurationBuilder<TData> message = this.network.RegisterMessage<TData>(name).SetDataType(this.Type);
            builder(message);

            return this;
        }
        public DataTypeConfigurationBuilder<TData> RegisterMessage(String name)
        {
            this.network.RegisterMessage(name).SetDataType(this.Type);

            return this;
        }
        #endregion

        #region DataTypeConfigurationBuilder Implmenetation
        public override DataTypeConfiguration Build(DynamicId id)
        {
            return new DataTypeConfiguration(
                id,
                this.Type,
                this.Writer.ToIDataWriter(),
                this.Reader);
        }
        #endregion
    }

}
