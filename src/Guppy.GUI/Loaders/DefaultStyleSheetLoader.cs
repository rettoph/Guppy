using Guppy.Attributes;
using Guppy.GUI.Constants;
using Guppy.GUI.Elements;
using Guppy.GUI.Providers;
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
            styleSheet.Configure(Selector.Create<Label>(ElementNames.TextInputLabel), manager =>
            {
                manager.Set(Property.Alignment, new Alignment(VerticalAlignment.Center, HorizontalAlignment.LeftFit), 100)
                    .Set(Property.Padding, new Padding(0, 0, 0, 0), 100);
            })
            .Configure(Selector.Create<TextInput>(), manager =>
            {
                manager.Set(Property.Alignment, Alignment.CenterLeft, 100);
            });
        }

        public void Load(IStyleSheetProvider styles)
        {
            // throw new NotImplementedException();
        }
    }
}
