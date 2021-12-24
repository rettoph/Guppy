using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Security;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Layerables
{
    public class Paddle : Positionable
    {
        public const Int32 Height = 15;
        public const Int32 Width = 75;

        public User User { get; set; }

        public Single Target { get; set; }


        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);
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
