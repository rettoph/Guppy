using Guppy.Demos.Pong.Entities;
using Guppy.Demos.Pong.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Demos.Pong.Scenes
{
    public sealed class GameScene : Scene
    {
        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Layers.Create<MainLayer>(Int32.MinValue, Int32.MaxValue);

            this.Entities.Create<Ball>();
        }
        #endregion
    }
}
