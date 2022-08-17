using Guppy.Loaders;
using Guppy.MonoGame;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.UI.Loaders
{
    internal sealed class NetworkUIServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDebugger, UsersDebugger>();
        }
    }
}
