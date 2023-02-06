using Guppy.Attributes;
using Guppy.MonoGame.UI.Constants;
using Guppy.MonoGame.UI.Providers;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.GameComponents
{
    internal sealed class BeginDebugImGuiBatch : SimpleDrawableGameComponent
    {
        private ImGuiBatch _imGuiBatch;

        public BeginDebugImGuiBatch(IImGuiBatchProvider batchs)
        {
            _imGuiBatch = batchs.Get(ImGuiBatchConstants.Debug);

            this.DrawOrder = int.MinValue;
        }

        public override void Draw(GameTime gameTime)
        {
            _imGuiBatch.Begin(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }
    }
}
