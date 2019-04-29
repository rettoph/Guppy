using Guppy.Collections;
using Guppy.Configurations;
using Guppy.Extensions;
using Guppy.Factories;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.Loggers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Guppy
{
    /// <summary>
    /// Main Guppy driver, used to initialize all
    /// guppy games.
    /// </summary>
    public class GuppyLoader
    {
        #region Private Fields
        private IServiceProvider _provider;
        private IServiceLoader[] _serviceLoader;

        private Boolean _initialized;
        #endregion

        #region Public Attributes
        public IServiceCollection Services;
        public ILogger Logger { get; }
        public GameFactory GameFactory { get; private set; }
        #endregion


        public GuppyLoader(ILogger logger, IServiceCollection services = null)
        {
            this.Logger = logger;

            _initialized = false;
            this.Services = services == null ? new ServiceCollection() : services;
            _serviceLoader = Assembly.GetEntryAssembly()
                .GetReferencedAssemblies()
                .ToList()
                .Append(Assembly.GetEntryAssembly().GetName())
                .SelectMany(an => Assembly.Load(an).GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IServiceLoader).IsAssignableFrom(t))
                .Select(t => Activator.CreateInstance(t) as IServiceLoader)
                .ToArray();

            // Add the logger to the service collection here...
            this.Services.AddSingleton<ILogger>(this.Logger);
        }

        public void Initialize()
        {
            if (_initialized)
                throw new Exception("Guppy instance already initialized!");

            // First, configure the services
            foreach (IServiceLoader serviceLoader in _serviceLoader)
                serviceLoader.ConfigureServiceCollection(this.Services);

            // Create a new service provider instance...
            _provider = this.Services.BuildServiceProvider();

            #region Provider & Loader Initialization
            // Now initialize the provider

            // Boot
            foreach (IServiceLoader serviceLoader in _serviceLoader)
                serviceLoader.Boot(_provider);

            // Ensure all loaders get loaded
            foreach (ILoader loader in _provider.GetLoaders())
                loader.Load();

            // Pre-Initialize
            foreach (IServiceLoader serviceLoader in _serviceLoader)
                serviceLoader.PreInitialize(_provider);
            // Initialize
            foreach (IServiceLoader serviceLoader in _serviceLoader)
                serviceLoader.Initialize(_provider);
            // Post-Initialize
            foreach (IServiceLoader serviceLoader in _serviceLoader)
                serviceLoader.PostInitialize(_provider);
            #endregion

            // Load required object from the provider
            this.GameFactory = _provider.GetRequiredService<GameFactory>();

            _initialized = true;
        }

        /// <summary>
        /// Automatically configure Guppy for a simple MonoGame client.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="window"></param>
        /// <param name="content"></param>
        public void ConfigureMonogame(GraphicsDeviceManager graphics, GameWindow window, ContentManager content)
        {
            this.Services.AddSingleton(graphics);
            this.Services.AddSingleton(window);
            this.Services.AddSingleton(content);
            this.Services.AddSingleton(graphics.GraphicsDevice);
            this.Services.AddSingleton(new SpriteBatch(graphics.GraphicsDevice));
        }
    }
}
