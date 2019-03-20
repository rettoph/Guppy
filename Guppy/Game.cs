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
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Guppy
{
    /// <summary>
    /// The main driver of a game. Controls all logic,
    /// manages the service provider, and handles all
    /// update/draw logic.
    /// </summary>
    public class Game
    {
        #region Private Fields
        private IServiceLoader[] _serviceLoaders;
        #endregion

        #region Proteced Attributes
        protected ILogger logger { get; private set; }
        protected ServiceCollection services { get; private set; }
        protected ServiceProvider provider { get; private set; }
        protected SceneCollection scenes { get; private set; }
        #endregion

        #region Public Attributes
        public Boolean Started { get; private set; }

        public Int32 Seed { get; private set; }
        #endregion

        #region Constructors
        public Game(Int32 seed = 1337)
        {
            this.services = new ServiceCollection();

            this.logger = new ConsoleLogger();
            this.Seed = seed;
        }
        #endregion

        #region Methods
        public void Start()
        {
            if (this.Started)
            {
                this.logger.LogError($"Unable to start Game. Already started.");
            }
            else
            {
                this.Boot();
                this.PreInitialize();
                this.Initialize();
                this.PostInitialize();

                this.Started = true;
            }
        }
        #endregion

        #region Initialization Methods
        protected virtual void Boot()
        {
            // Generate instances of all bound service loaders
            _serviceLoaders = Assembly.GetEntryAssembly()
                .GetReferencedAssemblies()
                .SelectMany(an => Assembly.Load(an).GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IServiceLoader).IsAssignableFrom(t))
                .Select(t => Activator.CreateInstance(t) as IServiceLoader)
                .ToArray();

            // Add core services to the collection...
            this.services.AddSingleton<Game>(this);
            this.services.AddSingleton<ILogger>(this.logger);
            this.services.AddSingleton<SceneCollection>();
            this.services.AddSingleton<Random>(new Random(this.Seed));
            this.services.AddScoped<GameScopeConfiguration>();
            this.services.AddScoped<LayerCollection>();
            this.services.AddScoped<EntityCollection>();
            this.services.AddScoped<EntityFactory>();
            this.services.AddScene<Scene>();

            // Add any default loaders
            this.services.AddLoader<StringLoader>();
            this.services.AddLoader<ColorLoader>();
            this.services.AddLoader<ContentLoader>();
            this.services.AddLoader<EntityLoader>();

            // Boot all service loaders
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.Boot(this.services);
        }

        protected virtual void PreInitialize()
        {
            // Build a new service provider...
            this.provider = this.services.BuildServiceProvider();

            // PreInitialize all service loaders
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.PreInitialize(this.provider);
        }

        protected virtual void Initialize()
        {
            // Load any required services
            this.scenes = this.provider.GetRequiredService<SceneCollection>();

            // Ensure all loaders get loaded
            foreach (ILoader loader in this.provider.GetLoaders())
                loader.Load();

            // Initialize all service loaders
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.Initialize(this.provider);
        }

        protected virtual void PostInitialize()
        {
            // PostInitialize all service loaders
            foreach (IServiceLoader serviceLoader in _serviceLoaders)
                serviceLoader.PostInitialize(this.provider);
        }
        #endregion

        #region Frame Methods
        public virtual void Draw(GameTime gameTime)
        {
            this.scenes.Draw(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            this.scenes.Update(gameTime);
        }
        #endregion
    }
}
