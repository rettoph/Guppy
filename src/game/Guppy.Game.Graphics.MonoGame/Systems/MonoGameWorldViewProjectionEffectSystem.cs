using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Systems;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Systems;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Systems;
using Guppy.Game.Graphics.Common;
using Guppy.Game.Graphics.Common.Effects;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Graphics.MonoGame.Systems
{
    public class MonoGameWorldViewProjectionEffectSystem(IGuppyEngine engine, ICamera2D camera) :
        IEngineSystem,
        IInitializeSystem,
        IDrawSystem
    {
        private readonly IGuppyEngine _engine = engine;
        private readonly ICamera2D _camera = camera;
        private IWorldViewProjectionEffect[] _effects = null!;

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.Initialize)]
        public void Initialize()
        {
            this._effects = this._engine.Resolve<IEnumerable<IWorldViewProjectionEffect>>().ToArray();
        }

        [SequenceGroup<DrawSequenceGroupEnum>(DrawSequenceGroupEnum.PreDraw)]
        public void Draw(GameTime gameTime)
        {
            foreach (var effect in this._effects)
            {
                effect.WorldViewProjection = this._camera.WorldViewProjection;
            }
        }
    }
}