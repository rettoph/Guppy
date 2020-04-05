using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Attributes;
using Guppy.Enums;
using Guppy.Utilities;
using Guppy.Extensions;
using Microsoft.Xna.Framework;

namespace Guppy.Demos.Pong.Entities
{
    [Service(Lifetime.Transient)]
    public class Ball : Entity
    {
        #region Private Fields
        private PrimitiveBatch _primitiveBatch;
        private Vector2 _position;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _primitiveBatch = provider.GetService<PrimitiveBatch>();
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _primitiveBatch.FillCircle(_position, 7, Color.Gray);
            _primitiveBatch.DrawCircle(_position, 7, Color.White);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _position += Vector2.One * 5;
        }
        #endregion
    }
}
