using Guppy.Collections;
using Guppy.Extensions;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.UI.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI
{
    public class UIServiceLoader : IServiceLoader
    {
        public void Boot(IServiceCollection services)
        {
            services.AddScoped<InputManager>(p =>
            {
                return p.GetService<EntityCollection>().Create("ui:input_manager") as InputManager;
            });
        }

        public void PreInitialize(IServiceProvider provider)
        {
            var entityLoader = provider.GetLoader<EntityLoader>();
            entityLoader.Register<Stage>("ui:stage", "ui_name:stage", "ui_description:stage");
            entityLoader.Register<InputManager>("ui:input_manager", "ui_name:input_manager", "ui_description:input_manager");
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
