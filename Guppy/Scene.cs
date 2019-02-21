using Guppy.Implementations;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public class Scene : LivingObject
    {
        #region Protected Attributes
        protected Game Game { get; private set; }
        #endregion

        #region Constructors
        public Scene(Game game, ILogger logger) : base(logger)
        {
            this.Game = game;
        }
        #endregion

        #region Frame Methods
        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
