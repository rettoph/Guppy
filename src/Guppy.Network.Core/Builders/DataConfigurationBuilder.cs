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
    public abstract class DataConfigurationBuilder : IPrioritizable
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

    public class DataConfigurationBuilder<TData> : DataConfigurationBuilder, IFluentPrioritizable<DataConfigurationBuilder<TData>>
        where TData : class, IData
    {
        #region Private Fields
        private Action<NetDataWriter, NetworkProvider, TData> _writer;
        private Func<NetDataReader, NetworkProvider, TData> _reader;
        #endregion

        #region Public Properties
        public Action<NetDataWriter, NetworkProvider, TData> Writer
        {
            get => _writer;
            set => this.SetWriter(value);
        }

        public Func<NetDataReader, NetworkProvider, TData> Reader
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
        public virtual DataConfigurationBuilder<TData> SetWriter(Action<NetDataWriter, NetworkProvider, TData> writer)
        {
            _writer = writer;

            return this;
        }
        #endregion

        #region SetReader Methods
        public virtual DataConfigurationBuilder<TData> SetReader(Func<NetDataReader, NetworkProvider, TData> reader)
        {
            _reader = reader;

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
