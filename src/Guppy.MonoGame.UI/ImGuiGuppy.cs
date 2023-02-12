using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI
{
    public abstract class ImGuiGuppy : FrameableGuppy
    {
        private ImGuiBatch? _imGui = default!;

        public override void Initialize(IServiceProvider provider)
        {
            base.Initialize(provider);

            _imGui = provider.GetService<ImGuiBatch>();
        }

        protected override void PreDraw(GameTime gameTime)
        {
            _imGui!.Begin(gameTime);

            base.PreDraw(gameTime);
        }

        protected override void PostDraw(GameTime gameTime)
        {
            base.PostDraw(gameTime);

            _imGui!.End();
        }
    }
}
