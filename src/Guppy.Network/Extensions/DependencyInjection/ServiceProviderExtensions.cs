using Guppy.DependencyInjection;
using Guppy.Network.Services;
using Guppy.Network.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Automatically return a broadcast directly from the
        /// ServiceProvider's Broadcasts service.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public static Broadcast GetBroadcast(this GuppyServiceProvider provider, UInt32 messageType)
            => provider.GetService<Broadcasts>()[messageType];
    }
}
