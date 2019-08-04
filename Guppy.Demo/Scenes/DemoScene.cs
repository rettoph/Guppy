using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Demo.Entities;
using Guppy.Demo.Layers;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;

namespace Guppy.Demo.Scenes
{
    public class DemoScene : Scene
    {
        protected override void Initialize()
        {
            base.Initialize();

            var layer = this.layers.Build<DemoLayer>(0);

            var e1 = this.entities.Build<DemoEntity>("entity:demo");
            this.entities.Build<DemoEntity>("entity:demo");
            this.entities.Build<DemoEntity>("entity:demo");
            this.entities.Build<DemoEntity>("entity:demo");
        }
    }
}
