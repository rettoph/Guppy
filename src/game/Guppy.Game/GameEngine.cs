using Autofac;
using Guppy.Core.Common.Extensions;
using Guppy.Engine;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game
{
    public class GameEngine : GuppyEngine, IGameEngine
    {
        private IGuppyDrawable[] _drawableComponents;
        private IGuppyUpdateable[] _updateableComonents;

        public ISceneService Scenes { get; private set; }

        public GameEngine(GuppyContext context, Action<ContainerBuilder>? builder = null) : base(context, builder)
        {
            this.Scenes = this.Resolve<ISceneService>();

            _drawableComponents = Array.Empty<IGuppyDrawable>();
            _updateableComonents = Array.Empty<IGuppyUpdateable>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            _drawableComponents = this.Components.OfType<IGuppyDrawable>().Sequence(DrawSequence.Draw).ToArray();
            _updateableComonents = this.Components.OfType<IGuppyUpdateable>().Sequence(UpdateSequence.Update).ToArray();
        }

        public void Draw(GameTime gameTime)
        {
            foreach (IGuppyDrawable component in _drawableComponents)
            {
                component.Draw(gameTime);
            }

            foreach (IScene scene in this.Scenes.GetAll())
            {
                scene.Draw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (IGuppyUpdateable component in _updateableComonents)
            {
                component.Update(gameTime);
            }

            foreach (IScene scene in this.Scenes.GetAll())
            {
                scene.Update(gameTime);
            }
        }

        public new GameEngine Start()
        {
            base.Start();

            return this;
        }

        IGameEngine IGameEngine.Start()
        {
            return this.Start();
        }
    }
}
