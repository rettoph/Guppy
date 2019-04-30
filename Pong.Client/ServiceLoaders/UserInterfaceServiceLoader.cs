using Guppy.Extensions;
using Guppy.Interfaces;
using Guppy.UI.Enums;
using Guppy.UI.Loaders;
using Guppy.UI.Styles;
using Guppy.UI.Utilities.Units.UnitValues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Client.ServiceLoaders
{
    public class UserInterfaceServiceLoader : IServiceLoader
    {
        public void ConfigureServiceCollection(IServiceCollection services)
        {
            // throw new NotImplementedException();
        }

        public void Boot(IServiceProvider provider)
        {
            var styleLoader = provider.GetLoader<StyleLoader>();
            styleLoader.Register("chat-input", new Style());
        }

        public void PreInitialize(IServiceProvider provider)
        {
            var styleLoader = provider.GetLoader<StyleLoader>();

            var chatInputBackground = new Texture2D(provider.GetRequiredService<GraphicsDevice>(), 1, 1);
            chatInputBackground.SetData<Color>(new Color[] { new Color(100, 100, 100, 100) });

            var chatInputStyles = styleLoader.GetValue("chat-input");
            chatInputStyles.Set<Texture2D>(StateProperty.Background, chatInputBackground);
            chatInputStyles.Set<Color>(StateProperty.TextColor, Color.White);
            chatInputStyles.Set<UnitValue>(GlobalProperty.PaddingTop, 1);
            chatInputStyles.Set<UnitValue>(GlobalProperty.PaddingLeft, 7);
            chatInputStyles.Set<UnitValue>(GlobalProperty.PaddingRight, 7);
        }

        public void Initialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void PostInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }


    }
}
