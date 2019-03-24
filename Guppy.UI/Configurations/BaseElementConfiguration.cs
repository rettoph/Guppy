using Guppy.Extensions;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.UI.Attributes;
using Guppy.UI.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Configurations
{
    public abstract class BaseElementConfiguration : IEntityData
    {
        #region Property Handles
        [ElementProperty]
        public String DebugColorHandle { get; set; }

        [ElementProperty]
        public String BackgroundTextureHandle { get; set; }

        [ElementProperty]
        public String ColorHandle { get; set; }

        [ElementProperty]
        public String FontHandle { get; set; }

        [ElementProperty]
        public Alignment? TextAlignment { get; set; }

        [ElementProperty]
        public BackgroundMethod? BackgroundMethod { get; set; }

        [ElementProperty]
        public SamplerState SamplerState { get; set; }
        #endregion

        #region Property Values
        protected internal Color      DebugColor        { get; private set; }
        protected internal Texture2D  BackgroundTexture { get; private set; }
        protected internal Color      Color             { get; private set; }
        protected internal SpriteFont Font              { get; private set; }
        #endregion



        protected internal BaseElementConfiguration()
        {
        }

        public virtual void Initialize(IServiceProvider provider)
        {
            var colorLoader = provider.GetLoader<ColorLoader>();
            this.DebugColor = colorLoader.GetValue(this.DebugColorHandle);
            this.Color = colorLoader.GetValue(this.ColorHandle);

            var contentLoader = provider.GetLoader<ContentLoader>();
            this.BackgroundTexture = contentLoader.Get<Texture2D>(this.BackgroundTextureHandle);
            this.Font = contentLoader.Get<SpriteFont>(this.FontHandle);
        }
    }
}
