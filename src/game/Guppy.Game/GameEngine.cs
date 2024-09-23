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
        private Action<GameTime>? _drawComponentsDelegate;
        private Action<GameTime>? _updateComponentsDelegate;

        public ISceneService Scenes { get; private set; }

        public GameEngine(GuppyContext context, Action<ContainerBuilder>? builder = null) : base(context, builder)
        {
            this.Scenes = this.Resolve<ISceneService>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            _drawComponentsDelegate = this.Components.SequenceDelegates<DrawComponentSequence, Action<GameTime>>();
            _updateComponentsDelegate = this.Components.SequenceDelegates<UpdateComponentSequence, Action<GameTime>>();
        }

        public void Draw(GameTime gameTime)
        {
            _drawComponentsDelegate?.Invoke(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            _updateComponentsDelegate?.Invoke(gameTime);
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
