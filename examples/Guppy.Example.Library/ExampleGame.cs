using Guppy.DependencyInjection;
using Guppy.Example.Library.Scenes;
using Guppy.Extensions.DependencyInjection;
using Guppy.Network;
using System;

namespace Guppy.Example.Library
{
    public class ExampleGame : Game
    {
        #region Private Fields
        private Peer _peer;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _peer);
        }
        #endregion

        #region Frame Methods
        protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
        #endregion
    }
}
