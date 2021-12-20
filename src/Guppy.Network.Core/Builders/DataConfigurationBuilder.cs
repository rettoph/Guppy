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
    public abstract class DataConfigurationBuilder : IFluentPrioritizable<DataConfigurationBuilder>
    {
        #region Protected Properties
        protected NetworkProviderBuilder network { get; private set; }
        #endregion

        #region Public Properties
        public Type Type { get; }

        public Int32 Priority { get; set; }
        #endregion

        #region Constructor
        protected DataConfigurationBuilder(Type type, NetworkProviderBuilder network)
        {
            this.network = network;

            this.Type = type;
        }
        #endregion

        #region Build Methods
        public abstract DataConfiguration Build(DynamicId id);
        #endregion
    }

    public class DataConfigurationBuilder<TData> : DataConfigurationBuilder
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
        internal DataConfigurationBuilder(NetworkProviderBuilder network) : base(typeof(TData), network)
        {
        }
        #endregion

        #region SetWriter Methods
        public DataConfigurationBuilder<TData> SetWriter(Action<NetDataWriter, TData> dataWriter)
        {
            _writer = dataWriter;

            return this;
        }
        #endregion

        #region SetReader Methods
        public DataConfigurationBuilder<TData> SetReader(Func<NetDataReader, TData> dataReader)
        {
            _reader = dataReader;

            return this;
        }
        #endregion

        #region RegisterMessage Methods
        public DataConfigurationBuilder<TData> RegisterNetworkMessage(Action<NetworkMessageConfigurationBuilder<TData>> builder)
        {
            NetworkMessageConfigurationBuilder<TData> message = this.network.RegisterNetworkMessage<TData>();
            builder(message);

            return this;
        }
        #endregion

        #region DataConfigurationBuilder Implmenetation
        public override DataConfiguration Build(DynamicId id)
        {
            return new DataConfiguration(
                id,
                this.Type,
                this.Writer.ToIDataWriter(),
                this.Reader);
        }
        #endregion
    }

}
