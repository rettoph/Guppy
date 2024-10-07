using Guppy.Core.Common.Attributes;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.Graphics.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Scene
{
    [AutoLoad]
    public sealed class MonoGameScreenComponent(IScreen screen) : ISceneComponent, IDrawableComponent
    {
        private readonly IScreen _screen = screen;

        [SequenceGroup<DrawComponentSequenceGroup>(DrawComponentSequenceGroup.PostDraw)]
        public void Draw(GameTime gameTime)
        {
            _screen.Camera.Update(gameTime);
        }
    }
}
