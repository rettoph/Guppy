﻿using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Engine;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game
{
    public class GameEngine : GuppyEngine, IGameEngine
    {
        private readonly ActionSequenceGroup<DrawSequenceGroupEnum, GameTime> _drawComponentsActions;
        private readonly ActionSequenceGroup<UpdateSequenceGroupEnum, GameTime> _updateComponentsActions;

        public ISceneService SceneService { get; private set; }

        public GameEngine(
            IEnumerable<IEnvironmentVariable> environment,
            Action<IGuppyRootBuilder>? builder = null) : base(environment, builder)
        {
            this._drawComponentsActions = new(true);
            this._updateComponentsActions = new(false);

            this.SceneService = this.Resolve<ISceneService>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            this._drawComponentsActions.Add(this.Systems.GetAll());
            this._updateComponentsActions.Add(this.Systems.GetAll());
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