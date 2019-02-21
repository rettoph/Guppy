using Guppy.Configurations;
using Guppy.Extensions;
using Guppy.Loggers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    /// <summary>
    /// The main driver of a game. Controls all logic,
    /// manages the service provider, and handles all
    /// update/draw logic.
    /// </summary>
    public class Game
    {
        #region Proteced Attributes
        protected ILogger Logger { get; private set; }
        protected ServiceCollection services { get; private set; }
        protected ServiceProvider provider { get; private set; }
        #endregion

        #region Public Attributes
        public Boolean Started { get; private set; }
        #endregion

        #region Constructors
        public Game()
        {
            this.services = new ServiceCollection();

            this.Logger = new ConsoleLogger();
        }
        #endregion

        #region Methods
        public void Start()
        {
            if(this.Started)
            {
                this.Logger.LogError($"Unable to start Game. Already started.");
            }
            else {
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
        }

        protected virtual void PreInitialize()
        {
            // Add core services to the collection...
            this.services.AddSingleton<Game>(this);
            this.services.AddSingleton<ILogger>(this.Logger);
            this.services.AddScoped<GameScopeConfiguration>();
            this.services.AddScene<Scene>();
        }

        protected virtual void Initialize()
        {
            // Build a new service provider...
            this.provider = this.services.BuildServiceProvider();
        }

        protected virtual void PostInitialize()
        {
        }
        #endregion
    }
}
