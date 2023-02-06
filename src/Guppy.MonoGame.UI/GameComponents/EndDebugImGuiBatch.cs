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
    internal sealed class EndDebugImGuiBatch : SimpleDrawableGameComponent
    {
        private ImGuiBatch _imGuiBatch;

        public EndDebugImGuiBatch(IImGuiBatchProvider batchs)
        {
            _imGuiBatch = batchs.Get(ImGuiBatchConstants.Debug);

            this.DrawOrder = int.MaxValue;
        }

        public override void Draw(GameTime gameTime)
        {
            _imGuiBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }
    }
}
