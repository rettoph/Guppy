using Guppy.Attributes;
using Guppy.GUI.Constants;
using Guppy.GUI.Elements;
using Guppy.GUI.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Loaders
{
    [AutoLoad]
    internal sealed class DefaultStyleSheetLoader : IStyleSheetLoader
    {
        public void Configure(IStyleSheet styleSheet)
        {
            styleSheet.Configure(Selector.Create<Label>(), label =>
            {
                label.Set(Property.Font, Fonts.Default);
            })
            .Configure(Selector.Create<TextInput>(), textInput =>
            {
                textInput.Set(Property.Alignment, Alignment.CenterLeft)
                    .Set(Property.BackgroundColor, Color.White)
                    .Set(Property.Width, 1f);

                textInput.Configure(Selector.Create<Label>(ElementNames.TextInputLabel), label =>
                {
                    label.Set(Property.Alignment, new Alignment(VerticalAlignment.Center, HorizontalAlignment.LeftFit))
                        .Set(Property.Padding, new Padding(0, 0, 0, 0))
                        .Set(Property.Color, Color.Black);
                });
            })
            .Configure(Selector.Create<Element>(ElementNames.ScrollBox), manager =>
            {
                manager.Set(Property.ScrollThumbColor, Color.DarkGray)
                    .Set(Property.ScrollTrackColor, Color.LightGray)
                    .Set(Property.ScrollTrackWidth, 10);
            });
        }

        public void Load(IStyleSheetProvider styles)
        {
            // throw new NotImplementedException();
        }
    }
}
