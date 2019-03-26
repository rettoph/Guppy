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
    /// <summary>
    /// Service loader to interface UI 
    /// functionality with core guppy
    /// </summary>
    public class UIServiceLoader : IServiceLoader
    {
        public void Boot(IServiceCollection services)
        {
            services.AddScoped<InputManager>(p =>
            {
                var entities = p.GetService<EntityCollection>();
                return entities.Create("ui:input_manager") as InputManager;
            });
        }

        public void PreInitialize(IServiceProvider provider)
        {
            var contentLoader = provider.GetLoader<ContentLoader>();
            contentLoader.Register("ui:font", "UI/font");

            var entityLoader = provider.GetLoader<EntityLoader>();
            entityLoader.Register<InputManager>("ui:input_manager", "ui:name:input_manager", "ui:description:input_manager");
            entityLoader.Register<Stage>("ui:stage", "ui:name:stage", "ui:description:stage");
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
