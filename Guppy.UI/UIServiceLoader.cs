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
            // throw new NotImplementedException();
        }

        public void PreInitialize(IServiceProvider provider)
        {
            var contentLoader = provider.GetLoader<ContentLoader>();
            contentLoader.Register("ui:font", "UI/font");
            contentLoader.Register("demo-button", "UI/demo-button");
            contentLoader.Register("demo-button-pressed", "UI/demo-button-pressed");

            var entityLoader = provider.GetLoader<EntityLoader>();
            entityLoader.Register<Stage>("ui:stage", "ui_name:stage", "ui_description:stage");
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
