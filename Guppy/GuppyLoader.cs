﻿using Guppy.Collections;
using Guppy.Configurations;
using Guppy.Extensions.DependencyInjection;
using Guppy.Factories;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.Loggers;
using Guppy.Utilities.Cameras;
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
using Guppy.Extensions.Linq;

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
        private IServiceLoader[] _serviceLoaders;

        private Boolean _initialized;
        #endregion

        #region Public Attributes
        public IServiceCollection Services;
        public ILogger Logger { get; }

        public GameCollection Games { get; private set; }

        public IServiceProvider Provider { get { return _provider; } }
        #endregion

        public GuppyLoader(ILogger logger, IServiceCollection services = null)
        {
            this.Logger = logger;

            _initialized = false;
            this.Services = services == null ? new ServiceCollection() : services;
            _serviceLoaders = this.getUniqueNestedReferencedAssemblies(Assembly.GetEntryAssembly())
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IServiceLoader).IsAssignableFrom(t))
                .Select(t => Activator.CreateInstance(t) as IServiceLoader)
                .ToArray();

            // Add core services here...
            this.Services.AddSingleton<GuppyLoader>(this);
            this.Services.AddSingleton<ILogger>(this.Logger);
        }

        public void Initialize()
        {
            if (_initialized)
                throw new Exception("Guppy instance already initialized!");

            // First, configure the services
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.ConfigureServiceCollection(this.Services);

            // Create a new service provider instance...
            _provider = this.Services.BuildServiceProvider();

            #region Provider & Loader Initialization
            // Now initialize the provider

            // Boot
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.Boot(_provider);

            // Ensure all loaders get loaded
            foreach (ILoader loader in _provider.GetLoaders())
                loader.Load();

            // Pre-Initialize
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.PreInitialize(_provider);
            // Initialize
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.Initialize(_provider);
            // Post-Initialize
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.PostInitialize(_provider);
            #endregion

            this.Games = _provider.GetRequiredService<GameCollection>();

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

            this.Services.AddTransient<Camera2D>();
            this.Services.AddTransient<BasicEffect>();
        }

        private List<Assembly> getUniqueNestedReferencedAssemblies(Assembly entry, List<Assembly> list = null)
        {
            if (list == null)
                list = new List<Assembly>();

            if(!list.Contains(entry))
            {
                list.Add(entry);

                foreach (Assembly child in entry.GetReferencedAssemblies().Select(an => Assembly.Load(an)))
                    this.getUniqueNestedReferencedAssemblies(child, list);
            }

            return list;
        }
    }
}
