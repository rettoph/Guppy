using Guppy.Common;
using Guppy.MonoGame.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    public abstract class FrameableGuppy : IGuppy
    {
        public IBus Bus { get; private set; }
        public IGameComponentService Components { get; private set; }

        public FrameableGuppy()
        {
            this.Bus = default!;
            this.Components = default!;
        }

        public virtual void Initialize(IServiceProvider provider)
        {
            this.Components = provider.GetRequiredService<IGameComponentService>();
            this.Bus = provider.GetRequiredService<IBus>();

            this.Bus.Initialize();
        }

        public virtual void Draw(GameTime gameTime)
        {
            this.Components.Draw(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            this.Components.Update(gameTime);

            this.Bus.Flush();
        }
    }
}
