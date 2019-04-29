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
            var contentLoader = provider.GetLoader<ContentLoader>();
            contentLoader.Register("ui:font", "UI/font");
            contentLoader.Register("ui:input", "UI/input");
            contentLoader.Register("ui:input:active", "UI/input-active");
            contentLoader.Register("ui:button", "UI/button");
            contentLoader.Register("ui:button:hovered", "UI/button-hovered");
            contentLoader.Register("ui:button:pressed", "UI/button-pressed");


            var entityLoader = provider.GetLoader<EntityLoader>();
            entityLoader.Register<Stage>("ui:stage", "ui_name:stage", "ui_description:stage");

            var styleLoader = provider.GetLoader<StyleLoader>();
            styleLoader.Register(typeof(TextInput), new Style());
            styleLoader.Register(typeof(TextButton), new Style());
        }

        public void PreInitialize(IServiceProvider provider)
        {
            var styleLoader = provider.GetLoader<StyleLoader>();
            var contentLoader = provider.GetLoader<ContentLoader>();

            var textInputStyle = styleLoader.GetValue(typeof(TextInput));
            textInputStyle.Set<Texture2D>(ElementState.Normal, StateProperty.Background, contentLoader.Get<Texture2D>("ui:input"));
            textInputStyle.Set<Texture2D>(ElementState.Active, StateProperty.Background, contentLoader.Get<Texture2D>("ui:input:active"));
            textInputStyle.Set<DrawType>(ElementState.Normal, StateProperty.BackgroundType, DrawType.Stretch);
            textInputStyle.Set<Alignment>(ElementState.Normal, StateProperty.TextAlignment, Alignment.CenterLeft);
            textInputStyle.Set<Color>(ElementState.Normal, StateProperty.TextColor, Color.Black);
            textInputStyle.Set<UnitValue>(GlobalProperty.PaddingTop, 7);
            textInputStyle.Set<UnitValue>(GlobalProperty.PaddingRight, 7);
            textInputStyle.Set<UnitValue>(GlobalProperty.PaddingBottom, 7);
            textInputStyle.Set<UnitValue>(GlobalProperty.PaddingLeft, 7);

            var buttonStyle = styleLoader.GetValue(typeof(TextButton));
            buttonStyle.Set<Texture2D>(ElementState.Normal, StateProperty.Background, contentLoader.Get<Texture2D>("ui:button"));
            buttonStyle.Set<Texture2D>(ElementState.Hovered, StateProperty.Background, contentLoader.Get<Texture2D>("ui:button:hovered"));
            buttonStyle.Set<Texture2D>(ElementState.Pressed, StateProperty.Background, contentLoader.Get<Texture2D>("ui:button:pressed"));
            buttonStyle.Set<DrawType>(ElementState.Normal, StateProperty.BackgroundType, DrawType.Tile);
            buttonStyle.Set<Alignment>(ElementState.Normal, StateProperty.TextAlignment, Alignment.CenterCenter);
            buttonStyle.Set<Color>(ElementState.Normal, StateProperty.TextColor, Color.White);
            buttonStyle.Set<UnitValue>(GlobalProperty.PaddingTop, 15);
            buttonStyle.Set<UnitValue>(GlobalProperty.PaddingRight, 15);
            buttonStyle.Set<UnitValue>(GlobalProperty.PaddingBottom, 15);
            buttonStyle.Set<UnitValue>(GlobalProperty.PaddingLeft, 15);
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
