using Guppy.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities
{
    internal class GraphicsHelper : Service
    {
        /// <summary>
        /// Single white pixel texture.
        /// </summary>
        public Texture2D Pixel { get; private set; }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            var graphics = provider.GetService<GraphicsDevice>();

            this.Pixel = new Texture2D(graphics, 1, 1);
            this.Pixel.SetData<Color>(new Color[] { Color.White });
        }
    }
}
