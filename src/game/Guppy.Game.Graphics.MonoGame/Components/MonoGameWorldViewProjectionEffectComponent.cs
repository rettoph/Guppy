using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.Graphics.Common;
using Guppy.Game.Graphics.Common.Effects;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Graphics.MonoGame.Components
{
    public class MonoGameWorldViewProjectionEffectComponent(ICamera2D camera, IEnumerable<IWorldViewProjectionEffect> effects) : IEngineComponent, IDrawableComponent
    {
        private readonly ICamera2D _camera = camera;
        private readonly IWorldViewProjectionEffect[] _effects = [.. effects];

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            // throw new NotImplementedException();
        }

        [SequenceGroup<DrawComponentSequenceGroup>(DrawComponentSequenceGroup.PreDraw)]
        public void Draw(GameTime gameTime)
        {
            foreach (var effect in _effects)
            {
                effect.WorldViewProjection = _camera.WorldViewProjection;
            }
        }
    }
}
