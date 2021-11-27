using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Builders
{
    public abstract class DataServiceBuilder<TDataService, TDataConfigurationBuilder, TConfiguration>
        where TDataConfigurationBuilder : DataConfigurationBuilder<TDataConfigurationBuilder, TConfiguration>, new()
    {
        protected List<TDataConfigurationBuilder> configurations { get; private set; }

        public DataServiceBuilder()
        {
            this.configurations = new List<TDataConfigurationBuilder>();
        }

        /// <summary>
        /// Register a new PacketConfiguration and return a new PacketConfigurationBuilder.
        /// </summary>
        /// <param name="id">Optional: A specific id to register this packet as. When undefined, a value will be auto generated instead.</param>
        /// <returns></returns>
        public TDataConfigurationBuilder Register()
        {
            TDataConfigurationBuilder builder = new TDataConfigurationBuilder();

            this.configurations.Add(builder);

            return builder;
        }

        public abstract TDataService Build();
    }
}
