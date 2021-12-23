using Guppy.EntityComponent.DependencyInjection;
using Guppy.Example.Library;
using Guppy.Example.Library.Scenes;
using Guppy.EntityComponent.Lists.Interfaces;
using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Example.Library.Layerables;

namespace Guppy.Example.Server.Scenes
{
    public class ServerExampleScene : ExampleScene
    {
        #region Private Fields
        private Ball _ball;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _ball = this.Layerables.Create<Ball>();
        }
        #endregion
    }
}
