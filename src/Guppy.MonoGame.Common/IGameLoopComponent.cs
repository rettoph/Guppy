using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Common
{
    public interface IGameLoopComponent
    {
        void Initialize(IGameLoop gameLoop);
    }
}
