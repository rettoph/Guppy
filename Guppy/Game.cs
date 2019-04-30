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
using Guppy.Enums;

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
        #endregion

        #region Public Attributes
        public Boolean Started { get; private set; }
        public Scene Scene { get; private set; }
        #endregion

        #region Constructors
        public Game(ILogger logger, IServiceProvider provider) : base(logger)
        {
            this.provider = provider;
        }

        
        #endregion

        #region Frame Methods
        public virtual void Draw(GameTime gameTime)
        {
            this.Scene.Draw(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            this.Scene.Update(gameTime);
        }
        #endregion

        #region Scene Methods
        public TScene CreateScene<TScene>()
            where TScene : Scene
        {
            var sceneScope = this.provider.CreateScope().ServiceProvider;
            sceneScope.GetRequiredService<GameScopeConfiguration>().Clone(this.provider.GetRequiredService<GameScopeConfiguration>());
            var scene = sceneScope.GetRequiredService<TScene>();

            // Initialize the new scene
            scene.TryBoot();
            scene.TryPreInitialize();
            scene.TryInitialize();
            scene.TryPostInitialize();

            return scene;
        }

        public Scene SetScene(Scene scene, Boolean disposeOld = true)
        {
            this.Scene?.setActive(false);

            if (disposeOld) // Dispose of the old scene if necessary
                this.Scene?.Dispose();


            this.Scene = scene;
            this.Scene.setActive(true);

            return this.Scene;
        }
        #endregion
    }
}
