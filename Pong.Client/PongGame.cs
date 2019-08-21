using Guppy;
using Guppy.Attributes;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Client
{
    [IsGame]
    class PongGame : Game
    {
        public PongGame(PooledFactory pooled)
        {
        }
    }
}
