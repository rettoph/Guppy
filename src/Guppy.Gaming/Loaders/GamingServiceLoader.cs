using Guppy.Gaming.Services;
using Guppy.Loaders;
using Guppy.Threading;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Loaders
{
    internal sealed class GamingServiceLoader<TGame> : IServiceLoader
        where TGame : Game
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGame<TGame>();

            services.AddSingleton<ISceneService, SceneService>();
            services.AddScoped<Bus>();
        }
    }
}
