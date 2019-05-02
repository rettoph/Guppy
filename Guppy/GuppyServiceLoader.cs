using Guppy.Collections;
using Guppy.Configurations;
using Guppy.Extensions;
using Guppy.Factories;
using Guppy.Interfaces;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    /// <summary>
    /// Core service provider that configures all guppy games.
    /// </summary>
    public class GuppyServiceLoader : IServiceLoader
    {
        public void ConfigureServiceCollection(IServiceCollection services)
        {
            // Add core services to the collection...
            services.AddScoped<GameScopeConfiguration>();
            services.AddScoped<SceneScopeConfiguration>();
            services.AddScoped<LayerCollection>();
            services.AddScoped<EntityCollection>();
            services.AddScoped<GameCollection>();
            services.AddScoped<EntityFactory>();
            services.AddScene<Scene>();
            services.AddGame<Game>();

            services.AddLoader<StringLoader>();
            services.AddLoader<ColorLoader>();
            services.AddLoader<ContentLoader>();
            services.AddLoader<EntityLoader>();
        }

        public void Boot(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void PreInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void Initialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void PostInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
