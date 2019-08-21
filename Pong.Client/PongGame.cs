using Guppy;
using Guppy.Attributes;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Utilities.Factories;

namespace Pong.Client
{
    [IsGame]
    class PongGame : Game
    {
        public PongGame(PooledFactory pooled, PongScene scene)
        {
            Console.WriteLine(scene.Id);
            scene.Dispose();
        }

        protected override void Initialize()
        {
            base.Initialize();

            Console.WriteLine(this.provider.GetService<Scene>().Id);
        }
    }
}
