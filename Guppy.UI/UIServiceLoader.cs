using Guppy.Collections;
using Guppy.Extensions;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.UI.Elements;
using Guppy.UI.Entities;
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

namespace Guppy.UI
{
    public class UIServiceLoader : IServiceLoader
    {
        public void ConfigureServiceCollection(IServiceCollection services)
        {
            services.AddLoader<StyleLoader>();
        }

        public void Boot(IServiceProvider provider)
        {
            var entityLoader = provider.GetLoader<EntityLoader>();
            entityLoader.Register<Stage>("ui:stage", "ui_name:stage", "ui_description:stage");

            var styleLoader = provider.GetLoader<StyleLoader>();
            styleLoader.Register(typeof(StageContent).FullName, new Style());
        }

        public void PreInitialize(IServiceProvider provider)
        {
            var styleLoader = provider.GetLoader<StyleLoader>();

            var stageContentStyle = styleLoader.GetValue(typeof(StageContent).FullName);
            stageContentStyle.Set<Color>(ElementState.Normal, StateProperty.OuterDebugColor, Color.Red);
            stageContentStyle.Set<Color>(ElementState.Hovered, StateProperty.OuterDebugColor, Color.Blue);
            stageContentStyle.Set<Color>(ElementState.Pressed, StateProperty.OuterDebugColor, Color.Green);
            stageContentStyle.Set<Color>(ElementState.Active, StateProperty.OuterDebugColor, Color.Orange);
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
