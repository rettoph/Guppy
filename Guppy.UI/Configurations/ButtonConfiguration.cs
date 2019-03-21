using Guppy.Extensions;
using Guppy.Loaders;
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

        public Texture2D Texture { get; private set; }
        public Texture2D HoverTexture { get; private set; }
        public Texture2D ActiveTexture { get; private set; }

        public ButtonConfiguration(String textureHandle, String hoverTextureHandle, String activeTextureHandle)
        {
            _textureHandle = textureHandle;
            _hoverTextureHandle = hoverTextureHandle;
            _activeTextureHandle = activeTextureHandle;
        }

        public override void Initialize(IServiceProvider provider)
        {
            base.Initialize(provider);

            var contentLoader = provider.GetLoader<ContentLoader>();

            this.Texture = contentLoader.Get<Texture2D>(_textureHandle);
            this.HoverTexture = contentLoader.Get<Texture2D>(_hoverTextureHandle);
            this.ActiveTexture = contentLoader.Get<Texture2D>(_activeTextureHandle);
        }
    }
}
