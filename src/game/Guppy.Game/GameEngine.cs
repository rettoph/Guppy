using Guppy.Core.Common;
using Guppy.Core.Common.Utilities;
using Guppy.Engine;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game
{
    public class GameEngine : GuppyEngine, IGameEngine
    {
        private readonly ActionSequenceGroup<DrawComponentSequenceGroupEnum, GameTime> _drawComponentsActions;
        private readonly ActionSequenceGroup<UpdateComponentSequenceGroupEnum, GameTime> _updateComponentsActions;

        public ISceneService Scenes { get; private set; }

        public GameEngine(
            GuppyEnvironment environment,
            Action<IGuppyScopeBuilder>? builder = null) : base(environment, builder)
        {
            this._drawComponentsActions = new(true);
            this._updateComponentsActions = new(false);

            this.Scenes = this.Resolve<ISceneService>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            this._drawComponentsActions.Add(this.Components);
            this._updateComponentsActions.Add(this.Components);
        }

        public void Draw(GameTime gameTime)
        {
            this._drawComponentsActions?.Invoke(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            this._updateComponentsActions.Invoke(gameTime);
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