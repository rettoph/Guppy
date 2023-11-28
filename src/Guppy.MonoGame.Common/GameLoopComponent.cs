using Guppy.Attributes;
using Guppy.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Common
{
    [Service<IGameLoopComponent>(ServiceLifetime.Singleton, true)]
    public abstract class GameLoopComponent : IGameLoopComponent
    {
        public virtual void Initialize(IGameLoop gameLoop)
        {
        }
    }
}
