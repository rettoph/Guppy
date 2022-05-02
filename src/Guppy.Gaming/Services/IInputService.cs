using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Services
{
    public interface IInputService : IEnumerable<Input>
    {
        internal void Update(GameTime gameTime);
    }
}
