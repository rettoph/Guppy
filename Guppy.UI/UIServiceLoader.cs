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
            entityLoader.Register<Element>("ui:element", "ui_name:element", "ui_description:element");
        }
    }
}
