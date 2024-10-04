using Autofac;
using Guppy.Core.Common;
using Guppy.Engine;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game
{
    public class GameEngine : GuppyEngine, IGameEngine
    {
        private readonly ActionSequenceGroup<DrawComponentSequenceGroup, GameTime> _drawComponentsActions;
        private readonly ActionSequenceGroup<UpdateComponentSequenceGroup, GameTime> _updateComponentsActions;

        public ISceneService Scenes { get; private set; }

        public GameEngine(GuppyContext context, Action<ContainerBuilder>? builder = null) : base(context, builder)
        {
            _drawComponentsActions = new(true);
            _updateComponentsActions = new(false);

            this.Scenes = this.Resolve<ISceneService>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            _drawComponentsActions.Add(this.Components);
            _updateComponentsActions.Add(this.Components);
        }

        public void Draw(GameTime gameTime)
        {
            _drawComponentsActions?.Invoke(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            _updateComponentsActions.Invoke(gameTime);
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
