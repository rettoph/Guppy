using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.ImGui.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Engine
{
    [AutoLoad]
    [Sequence<InitializeSequence>(InitializeSequence.Initialize)]
    internal class EndImGuiComponent : EngineComponent, IDrawableComponent
    {
        private readonly IImguiBatch _batch;

        public EndImGuiComponent(IImguiBatch batch)
        {
            _batch = batch;
        }

        [Sequence<DrawComponentSequence>(DrawComponentSequence.PostDraw)]
        public void Draw(GameTime gameTime)
        {
            _batch.End();
        }
    }
}
