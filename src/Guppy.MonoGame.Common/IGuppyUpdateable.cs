using Guppy.Common;
using Guppy.MonoGame.Common.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Common
{
    public interface IGuppyUpdateable : ISequenceable<UpdateSequence>
    {
        void Update(GameTime gameTime);
    }
}
