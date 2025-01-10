using Guppy.Engine.Common;
using Guppy.Game.Common.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Common
{
    public interface IGameEngine : IGuppyEngine
    {
        ISceneService Scenes { get; }

        new IGameEngine Start();

        void Draw(GameTime gameTime);

        void Update(GameTime gameTime);
    }
}