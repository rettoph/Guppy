using Guppy.DependencyInjection;
using Guppy.Lists;
using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Lists
{
    public class PipeList : FactoryServiceList<IPipe>
    {
        internal IChannel channel;

        protected override T Create<T>(ServiceProvider provider, ServiceConfigurationKey configurationKey, Action<T, ServiceProvider, ServiceConfiguration> setup = null, Guid? id = null)
        {
            return base.Create<T>(provider, configurationKey, (pipe, p, c) =>
            {
                pipe.Channel = this.channel;

                setup?.Invoke(pipe, p, c);
            }, id);
        }
    }
}
