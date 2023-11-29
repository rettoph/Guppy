using Guppy.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Common
{
    public interface IGame : IDisposable
    {
        IGuppyProvider Guppies { get; }

        void Initialize();

        void Update(GameTime gameTime);

        void Draw(GameTime gameTime);
    }
}
