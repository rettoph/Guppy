using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Attributes;
using Guppy.Common.Utilities;
using Guppy.GUI.Constants;
using Guppy.GUI.Elements;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Loaders
{
    [AutoLoad]
    internal sealed class StageLoader_Default : IStageLoader, ISortable
    {
        public BlockList StageBlockList => BlockList.AllowAll;

        public bool GetOrder(Type type, out int order)
        {
            order = -1;
            return true;
        }

        public void Load(Stage stage)
        {
            stage.StyleSheet.Configure(Selector.Create<Label>(), label =>
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
    }
}
