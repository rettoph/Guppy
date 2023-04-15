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
            }).Configure(Selector.Create<TextInput>(), textInput =>
            {
                textInput.Set(Property.Alignment, Alignment.CenterLeft)
                    .Set(Property.BackgroundColor, Color.White)
                    .Set(Property.Width, 1f)
                    .Set(Property.BorderColor, ElementState.Focused, new Color(134, 183, 254))
                    .Set(Property.BorderThickness, ElementState.Focused, 2)
                    .Set(Property.BorderColor, Color.Gray)
                    .Set(Property.BorderThickness, 1)
                    .Set(Property.Padding, new Padding(5, 7, 5, 7));

                textInput.Configure(Selector.Create<Label>(ElementNames.TextInputLabel), label =>
                {
                    label.Set(Property.Alignment, new Alignment(VerticalAlignment.Center, HorizontalAlignment.LeftFit))
                        .Set(Property.Padding, new Padding(0, 0, 0, 0))
                        .Set(Property.Color, Color.Black);
                });
            }).Configure(Selector.Create<Element>(ElementNames.ScrollBox), manager =>
            {
                manager.Set(Property.ScrollThumbColor, Color.DarkGray)
                    .Set(Property.ScrollTrackColor, Color.LightGray)
                    .Set(Property.ScrollTrackWidth, 10);
            });
        }

        public void Load(IStyleSheetProvider styles)
        {
            Unit TerminalInputHeight = 35;

            styles.Get(StyleSheets.Guppy).Configure(Selector.Create<Stage>(ElementNames.Terminal), terminal =>
            {
                terminal.Configure(Selector.Create<Element>(ElementNames.TerminalOutputContainer), output =>
                {
                    output.Set(Property.Height, 1f - TerminalInputHeight)
                        .Set(Property.Width, 1f)
                        .Set(Property.Padding, new Padding(7, 7, 7, 7))
                        .Set(Property.Alignment, Alignment.BottomLeft);

                    output.Configure(Selector.Create<Label>(ElementNames.TerminalOutput), label =>
                    {
                        label.Set(Property.Padding, new Padding(0, 0, 0, 0));
                    });
                }).Configure(Selector.Create<TextInput>(ElementNames.TerminalInput), input =>
                {
                    input.Set(Property.BackgroundColor, Color.Black)
                        .Set(Property.Height, TerminalInputHeight);

                    input.Configure(Selector.Create<Label>(ElementNames.TextInputLabel), label =>
                    {
                        label.Set(Property.Color, Color.White);
                    });
                });
            });
        }
    }
}
