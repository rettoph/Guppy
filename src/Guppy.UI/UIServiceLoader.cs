using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Services;
using Guppy.UI.Lists;
using Guppy.UI.Components;
using Guppy.UI.Drivers.Entities;
using Guppy.UI.Entities;
using Guppy.UI.Extensions.DependencyInjection;
using Guppy.UI.Interfaces;
using Guppy.UI.Layers;
using Guppy.UI.Utilities;
using Guppy.Utilities;
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
            // Add factories...
            services.AddFactory<GraphicsHelper>(p => new GraphicsHelper());
            services.AddFactory<StageLayer>(p => new StageLayer());
            services.AddFactory<ComponentList<IComponent>>(p => new ComponentList<IComponent>());
            services.AddFactory<Container<IComponent>>(p => new Container<IComponent>());
            services.AddFactory<StackContainer<IComponent>>(p => new StackContainer<IComponent>());
            services.AddFactory<Cursor>(p => new Cursor());
            services.AddFactory<Stage>(p => new Stage());

            // Setup service lifetimes
            services.AddSingleton<GraphicsHelper>();
            services.AddTransient<StageLayer>();
            services.AddTransient<ComponentList<IComponent>>();
            services.AddTransient<Container<IComponent>>();
            services.AddTransient<StackContainer<IComponent>>();
            services.AddScoped<Cursor>();
            services.AddTransient<Stage>();

            // Register UI Components...
            services.AddUIComponent<Component>(p => new Component());
            services.AddUIComponent<TextComponent>(p => new TextComponent());
            services.AddUIComponent<Label>(p => new Label());
            services.AddUIComponent<TextInput>(p => new TextInput());
            services.AddUIComponent<TextButton>(p => new TextButton());
            services.AddUIComponent<ScrollContainer>(p => new ScrollContainer());
            services.AddUIComponent<Paginator>(p => new Paginator());
            services.AddUIComponent<Page>(p => new Page());
            

            //Register Drivers...
            services.AddDriver<MouseIndicatorDriver>(p => new MouseIndicatorDriver());
            services.BindDriver<Cursor, MouseIndicatorDriver>();

            // Register Content
            services.AddConfiguration<ContentService>((content, p, c) =>
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
