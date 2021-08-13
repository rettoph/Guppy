using Guppy.DependencyInjection;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public static class Constants
    {
        public static class ServiceConfigurationKeys
        {
            public static readonly ServiceConfigurationKey GameBaseCacheKey = ServiceConfigurationKey.From<Game>();
            public static readonly ServiceConfigurationKey SceneBaseCacheKey = ServiceConfigurationKey.From<Scene>();
            public static readonly ServiceConfigurationKey TransientSpritebatch = ServiceConfigurationKey.From<SpriteBatch>("service:spritebatch:transient");
            public static readonly ServiceConfigurationKey TransientCamera = ServiceConfigurationKey.From<Camera2D>("service:camera:transient");
        }
    }
}
