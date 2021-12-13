using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Builders
{
    public abstract class DataServiceBuilder<TDataService, TConfiguration>
    {
        protected List<IDataConfigurationBuilder<TConfiguration>> configurations { get; private set; }

        public DataServiceBuilder()
        {
            this.configurations = new List<IDataConfigurationBuilder<TConfiguration>>();
        }

        public abstract TDataService Build();
    }
}
