using System;
using Guppy.Extensions;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.UI.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Configurations
{
    public class ElementConfiguration : BaseElementConfiguration
    {
        public StateElementConfiguration Hovered { get; set; }
        public StateElementConfiguration Active { get; set; }

        public ElementConfiguration(
            String backgroundTextureHandle = null,
            String colorHandle = "black",
            String fontHandle = "ui:font",
            Alignment textAlignment = Alignment.TopLeft)
        {
            this.DebugColorHandle = "ui:debug:normal";
            this.BackgroundTextureHandle = backgroundTextureHandle;
            this.ColorHandle = colorHandle;
            this.FontHandle = fontHandle;
            this.TextAlignment = textAlignment;
            this.BackgroundMethod = Enums.BackgroundMethod.Stretch;
            this.SamplerState = SamplerState.PointWrap;

            // Create new placeholder hovered and active configurations
            this.Hovered = new StateElementConfiguration();
            this.Active = new StateElementConfiguration();
        }

        public override void Initialize(IServiceProvider provider)
        {
            base.Initialize(provider);

            this.Hovered.DebugColorHandle = "ui:debug:hovered";
            this.Hovered.SetParent(this);
            this.Hovered.Initialize(provider);

            this.Active.DebugColorHandle = "ui:debug:active";
            this.Active.SetParent(this);
            this.Active.Initialize(provider);
        }

        public BaseElementConfiguration GetConfiguration(ElementState state)
        {
            switch (state)
            {
                case ElementState.Normal:
                    return this;
                case ElementState.Hovered:
                    return this.Hovered;
                case ElementState.Active:
                    return this.Active;
            }

            return this;
        }
    }
}