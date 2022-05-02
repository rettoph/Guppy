using Guppy.Loaders;
using Guppy.Network.Constants;
using Guppy.Network.Messages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Loaders
{
    internal sealed class BusServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBusMessage<NetIncomingMessage<UserActionMessage>>(BusConstants.PeerQueuePriority);
        }
    }
}
