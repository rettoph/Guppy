using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Services
{
    public interface IGameComponentService : IEnumerable<IGameComponent>
    {
        IEnumerable<IDrawable> Drawables { get; }
        IEnumerable<IUpdateable> Updateables { get; }

        void Draw(GameTime gameTime);
        void Update(GameTime gameTime);
    }
}
