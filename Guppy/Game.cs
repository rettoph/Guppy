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
using Guppy.Implementations;

namespace Guppy
{
    /// <summary>
    /// The main driver of a game. Controls all logic,
    /// manages the service provider, and handles all
    /// update/draw logic.
    /// </summary>
    public class Game : Initializable
    {
        #region Proteced Attributes
        protected IServiceProvider provider { get; private set; }
        protected SceneCollection scenes { get; private set; }
        #endregion

        #region Public Attributes
        public Boolean Started { get; private set; }
        #endregion

        #region Constructors
        public Game(ILogger logger, IServiceProvider provider) : base(logger)
        {
            this.provider = provider;

            // Load any required services
            this.scenes = this.provider.GetRequiredService<SceneCollection>();
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
