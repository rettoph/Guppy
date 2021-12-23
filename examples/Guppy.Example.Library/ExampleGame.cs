using Guppy.EntityComponent.DependencyInjection;
using Guppy.Example.Library.Scenes;
using Guppy.Network;
using Microsoft.Xna.Framework;
using System;

namespace Guppy.Example.Library
{
    public class ExampleGame : Game
    {
        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.Scenes.Create<ExampleScene>();
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        #endregion
    }
}
