using Guppy.Attributes;
using Guppy.Interfaces;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Pong.Client.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Client.ServiceLoaders
{
    [IsServiceLoader]
    public class PongServiceLoader : IServiceLoader
    {
        public void ConfigureProvider(IServiceProvider provider)
        {
            var entities = provider.GetRequiredService<EntityLoader>();
            entities.TryRegister<Ball>("pong:ball");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // throw new NotImplementedException();
        }
    }
}
