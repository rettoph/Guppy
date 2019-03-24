using Guppy.Collections;
using Guppy.Extensions;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.UI.Configurations;
using Guppy.UI.Entities;
using Guppy.UI.Extensions.DependencyInjection;
using Guppy.UI.Layers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;

namespace Guppy.UI
{
    public class UIServiceLoader : IServiceLoader
    {
        public void Boot(IServiceCollection services)
        {
            services.AddLayer<UILayer>();

            services.AddScoped<InputManager>(p =>
            {
                var entities = p.GetService<EntityCollection>();
                return entities.Create("ui:input_manager") as InputManager;
            });
        }

        public void Initialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void PostInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void PreInitialize(IServiceProvider provider)
        {
            var contentLoader = provider.GetLoader<ContentLoader>();
            contentLoader.Register("ui:font", "UI/font");

            var colorLoader = provider.GetLoader<ColorLoader>();
            colorLoader.Register("ui:debug:normal" , Color.Red);
            colorLoader.Register("ui:debug:hovered", Color.Blue);
            colorLoader.Register("ui:debug:active" , Color.Green);
            colorLoader.Register("black", Color.Orange);

            var entityLoader = provider.GetLoader<EntityLoader>();
            entityLoader.Register<InputManager>("ui:input_manager", "ui_name:input_manager", "ui_description:input_manager");
            entityLoader.AddElement<Image>("ui:image", "ui_name:image", "ui_description:image", new ElementConfiguration());
        }
    }
}
