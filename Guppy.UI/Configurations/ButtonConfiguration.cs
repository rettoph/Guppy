using Guppy.Extensions;
using Guppy.Loaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Configurations
{
    public class ButtonConfiguration : ElementConfiguration
    {
        private readonly String _textureHandle;
        private readonly String _hoverTextureHandle;
        private readonly String _activeTextureHandle;
        private readonly String _fontHandle;
        private readonly String _colorHandle;

        public Texture2D Texture { get; private set; }
        public Texture2D HoverTexture { get; private set; }
        public Texture2D ActiveTexture { get; private set; }
        public SpriteFont Font { get; private set; }
        public Color Color;

        public ButtonConfiguration(String textureHandle, String hoverTextureHandle, String activeTextureHandle, String fontHandle, String colorHandle = null)
        {
            _textureHandle = textureHandle;
            _hoverTextureHandle = hoverTextureHandle;
            _activeTextureHandle = activeTextureHandle;
            _fontHandle = fontHandle;
            _colorHandle = colorHandle;
        }

        public override void Initialize(IServiceProvider provider)
        {
            base.Initialize(provider);

            // Load button content
            var contentLoader = provider.GetLoader<ContentLoader>();
            this.Texture = contentLoader.Get<Texture2D>(_textureHandle);
            this.HoverTexture = contentLoader.Get<Texture2D>(_hoverTextureHandle);
            this.ActiveTexture = contentLoader.Get<Texture2D>(_activeTextureHandle);
            this.Font = contentLoader.Get<SpriteFont>(_fontHandle);

            // Load button colors
            var colorLoader = provider.GetLoader<ColorLoader>();

            this.Color = _colorHandle == null ? Color.Black : colorLoader[_colorHandle];
        }
    }
}
