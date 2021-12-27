using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Security;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Entities
{
    public class Paddle : Positionable
    {
        public const Int32 Height = 15;
        public const Int32 Width = 75;

        private List<Paddle> _paddles;

        public User User { get; set; }

        public Single Target { get; set; }

        public RectangleF Bounds => new RectangleF(this.position.X, this.position.Y, Paddle.Width, Paddle.Height);


        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _paddles);
            _paddles.Add(this);
        }

        protected override void Release()
        {
            base.Release();

            _paddles.Remove(this);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Single speed = (Single)gameTime.ElapsedGameTime.TotalSeconds * 4;
            this.Position = new Vector2(MathHelper.Lerp(this.Position.X, this.Target, speed), this.Position.Y);
            this.MasterPosition = new Vector2(MathHelper.Lerp(this.MasterPosition.X, this.Target, speed), this.MasterPosition.Y);
        }
    }
}
