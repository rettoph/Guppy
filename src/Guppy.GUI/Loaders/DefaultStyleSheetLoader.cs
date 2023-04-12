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
            styleSheet.Configure(Selector.Create<TextInput>(), textInput =>
            {
                textInput.Configure(Selector.Create<Label>(), label =>
                {
                    label.Set(Property.Alignment, new Alignment(VerticalAlignment.Center, HorizontalAlignment.LeftFit))
                        .Set(Property.Padding, new Padding(0, 0, 0, 0))
                        .Set(Property.BackgroundColor, ElementState.Hovered, Color.Green);
                });

                textInput.Set(Property.Alignment, Alignment.CenterLeft);
            });
        }

        public void Load(IStyleSheetProvider styles)
        {
            // throw new NotImplementedException();
        }
    }
}
