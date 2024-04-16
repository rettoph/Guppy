using Guppy.Engine.Common.Providers;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Common
{
    public interface IGame : IDisposable
    {
        IGuppyProvider Guppies { get; }

        void Initialize();

        void Update(GameTime gameTime);

        void Draw(GameTime gameTime);
    }
}
