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

namespace Guppy.Example.Library.Entities
{
    public class Ball : Positionable
    {
        #region Private Fields
        private List<Paddle> _paddles;
        #endregion

        #region Public Properties
        public Single Radius { get; set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _paddles);

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

            if(this.Awake == false)
            {
                return;
            }

            this.position += this.velocity * (Single)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 deltaVel = Vector2.One;

            if(this.position.X > Constants.WorldWidth - this.Radius)
            {
                deltaVel.X = -1;
                this.position.X = Constants.WorldWidth - this.Radius;
            }
            else if(this.position.X < this.Radius)
            {
                deltaVel.X = -1;
                this.position.X = this.Radius;
            }

            if (this.position.Y > Constants.WorldHeight - this.Radius)
            {
                deltaVel.Y *= -1;
                this.position.Y = Constants.WorldHeight - this.Radius;
            }
            else if (this.position.Y < this.Radius)
            {
                deltaVel.Y *= -1;
                this.position.Y = this.Radius;
            }

            foreach(Paddle paddle in _paddles)
            {
                if(this.CheckOverlap(paddle, out Vector2 nearestPoint))
                {
                    deltaVel.Y = -1;
                    this.position.Y = nearestPoint.Y + (this.Radius * (this.velocity.Y < 0 ? 1 : -1));
                }
            }

            this.velocity *= deltaVel;
        }
        #endregion

        #region Helper Methods
        public Boolean CheckOverlap(Paddle paddle, out Vector2 nearestPoint)
        {
            // Find the nearest point on the paddle to the current ball
            // Based off of: https://www.geeksforgeeks.org/check-if-any-point-overlaps-the-given-circle-and-rectangle/
            nearestPoint = new Vector2(
                x: Math.Max(paddle.Bounds.Left, Math.Min(this.position.X, paddle.Bounds.Right)),
                y:  Math.Max(paddle.Bounds.Top, Math.Min(this.position.Y, paddle.Bounds.Bottom)));

            Single distance = Vector2.Distance(this.position, nearestPoint);

            return distance <= this.Radius;
        }
        #endregion
    }
}
