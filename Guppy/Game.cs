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
using System.Threading;

namespace Guppy
{
    /// <summary>
    /// The main driver of a game. Controls all logic,
    /// manages the service provider, and handles all
    /// update/draw logic.
    /// </summary>
    public class Game : Driven
    {
        #region Private Fields
        private Thread _thread;
        private Boolean _running;
        private Boolean _draw;
        #endregion

        #region Public Attributes
        public Scene Scene { get; private set; }
        #endregion

        #region Constructors
        public Game(IServiceProvider provider) : base(provider)
        {
        }
        #endregion

        #region Frame Methods
        protected override void draw(GameTime gameTime)
        {
            this.Scene.Draw(gameTime);
        }

        protected override void update(GameTime gameTime)
        {
            this.Scene.Update(gameTime);
        }
        #endregion

        #region Scene Methods
        public TScene CreateScene<TScene>(params Object[] args)
            where TScene : Scene
        {
            var sceneScope = this.provider.CreateScope();
            var sceneProvider = sceneScope.ServiceProvider;
            sceneProvider.GetRequiredService<GameScopeConfiguration>().Clone(this.provider.GetRequiredService<GameScopeConfiguration>());
            var sceneFactory = sceneProvider.GetService<SceneFactory<TScene>>();

            if (sceneFactory == null)
                throw new Exception($"Unknown Scene<{typeof(TScene).Name}>. Please ensure the scene is registered via service provider.");

            var scene = sceneFactory.CreateCustom(sceneProvider, args);

            // Initialize the new scene
            scene.setScope(sceneScope);
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

        /// <summary>
        /// Start a new thread loop for this game 
        /// that is independent from all other games. 
        /// </summary>
        public void StartAsyc(Boolean draw = false)
        {
            this.logger.LogInformation("Starting async game loop.");

            _draw = draw;
            _running = true;
            _thread = new Thread(new ThreadStart(this.loop));
            _thread.Start();
        }

        private void loop()
        {
            
            DateTime start = DateTime.Now;
            DateTime last = DateTime.Now;
            GameTime time;

            while (_running)
            {
                Thread.Sleep(16);

                time = new GameTime(DateTime.Now.Subtract(start), DateTime.Now.Subtract(last));
                this.update(time);

                if (_draw)
                    this.draw(time);

                last = DateTime.Now;
            }

            this.logger.LogInformation("Stopping async game loop.");
        }

        public void Stop()
        {
            _running = false;
        }
        #endregion
    }
}
