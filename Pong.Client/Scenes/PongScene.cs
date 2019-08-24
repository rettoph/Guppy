using Guppy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Client.Scenes
{
    public class PongScene : Scene
    {
        protected override void Initialize()
        {
            base.Initialize();

            this.entities.Create("pong:ball");
        }
    }
}
