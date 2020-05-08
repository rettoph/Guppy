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
using Guppy.UI.Utilities;
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
            services.AddSingleton<GraphicsHelper>(p => new GraphicsHelper());
            services.AddTransient<StageLayer>(p => new StageLayer());
            services.AddTransient<ComponentCollection>(p => new ComponentCollection());

            // Register Entities...
            services.AddEntity<Cursor>(p => new Cursor());
            services.AddEntity<Stage>(p => new Stage());

            // Register UI Components...
            services.AddTransient<Component>(p => new Component());
            services.AddTransient<TextComponent>(p => new TextComponent());
            services.AddTransient<Label>(p => new Label());
            services.AddTransient<TextInput>(p => new TextInput());
            services.AddTransient<TextButton>(p => new TextButton());
            services.AddTransient<Container>(p => new StackContainer());
            services.AddTransient<StackContainer>(p => new StackContainer());
            services.AddTransient<ScrollContainer>(p => new ScrollContainer());
            services.AddTransient<Paginator>(p => new Paginator());

            //Register Drivers...
            services.AddDriver<MouseIndicatorDriver>(p => new MouseIndicatorDriver());
            services.BindDriver<Cursor, MouseIndicatorDriver>();

            // Register Content
            services.AddConfiguration<ContentLoader>((content, p, c) =>
            {
                content.TryRegister("ui:font", "UI/Font");
            });

            // Register default setup
            services.AddConfiguration<ITextComponent>((e, p, c) =>
            {
                e.Font = p.GetContent<SpriteFont>("ui:font");
                e.Color = Color.Black;
            }, -5);
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
