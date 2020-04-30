using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.UI.Collections;
using Guppy.UI.Components;
using Guppy.UI.Drivers.Entities;
using Guppy.UI.Entities;
using Guppy.UI.Interfaces;
using Guppy.UI.Layers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI
{
    [AutoLoad]
    internal sealed class UIServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            
            services.AddTransient<StageLayer>(p => new StageLayer());
            services.AddTransient<ComponentCollection>(p => new ComponentCollection());

            // Register Entities...
            services.AddEntity<Indicator>(p => new Indicator());
            services.AddEntity<Stage>(p => new Stage());

            // Register UI Components...
            services.AddTransient<Label>(p => new Label());
            services.AddTransient<TextInput>(p => new TextInput());

            //Register Drivers...
            services.AddDriver<MouseIndicatorDriver>(p => new MouseIndicatorDriver());
            services.BindDriver<Indicator, MouseIndicatorDriver>();

            // Register Content
            services.AddConfiguration<ContentLoader>((content, p, c) =>
            {
                content.TryRegister("ui:font", "UI/Font");
            });

            // Register default setup
            services.AddConfiguration<ITextElement>((e, p, c) =>
            {
                e.Font = p.GetContent<SpriteFont>("ui:font");
                e.Color = Color.Black;
            });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
