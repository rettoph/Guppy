using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.MonoGame.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Scene
{
    [SequenceGroup<DrawSequence>(DrawSequence.PreDraw)]
    internal sealed class ScreenComponent : ISceneComponent, IDrawableComponent
    {
        private readonly IScreen _screen;

        public ScreenComponent(IScreen screen)
        {
            _screen = screen;
        }

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IScene scene)
        {
            //
        }

        [SequenceGroup<DrawComponentSequenceGroup>(DrawComponentSequenceGroup.PostDraw)]
        public void Draw(GameTime gameTime)
        {
            _screen.Camera.Update(gameTime);
        }
    }
}
