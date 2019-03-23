using Guppy.Collections;
using Guppy.Extensions;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.UI.Entities;
using Guppy.UI.Layers;
using Microsoft.Extensions.DependencyInjection;
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
            var entityLoader = provider.GetLoader<EntityLoader>();
            entityLoader.Register<InputManager>("ui:input_manager", "ui_name:input_manager", "ui_description:input_manager");
            entityLoader.Register<Element>("ui:element", "ui_name:element", "ui_description:element");
        }
    }
}
