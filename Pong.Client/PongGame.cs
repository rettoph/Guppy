using Guppy;
using Pong.Client.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Client
{
    public class PongGame : Game
    {
        protected override void Initialize()
        {
            base.Initialize();

            this.scenes.Create<PongScene>();
        }
    }
}
