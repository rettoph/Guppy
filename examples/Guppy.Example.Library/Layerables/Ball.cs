using Guppy.EntityComponent.DependencyInjection;
using Guppy.Example.Library.Messages;
using Guppy.Network;
using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
using Guppy.Network.Services;
using Guppy.Threading.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Example.Library.Layerables
{
    public class Ball : Positionable
    {
        #region Public Properties
        public Single Radius { get; set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.position = new Vector2(10, 10);
            this.velocity = new Vector2(50, 80);
            this.Radius = 7f;
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.position += this.velocity * (Single)gameTime.ElapsedGameTime.TotalSeconds;

            if(this.position.X > Constants.WorldWidth - this.Radius)
            {
                this.velocity.X *= -1;
                this.position.X = Constants.WorldWidth - this.Radius;
            }
            else if(this.position.X < this.Radius)
            {
                this.velocity.X *= -1;
                this.position.X = this.Radius;
            }

            if (this.position.Y > Constants.WorldHeight - this.Radius)
            {
                this.velocity.Y *= -1;
                this.position.Y = Constants.WorldHeight - this.Radius;
            }
            else if (this.position.Y < this.Radius)
            {
                this.velocity.Y *= -1;
                this.position.Y = this.Radius;
            }
        }
        #endregion
    }
}
