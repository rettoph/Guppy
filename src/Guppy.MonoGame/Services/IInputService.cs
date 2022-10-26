using Guppy.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Services
{
    public interface IInputService : IBusPublisher, IEnumerable<IInput>
    {
        void Update(GameTime gameTime);
    }
}
