using Guppy.Attributes;
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
    internal class StageLoader_Terminal : IStageLoader
    {
        public BlockList StageBlockList => BlockList.CreateWhitelist(ElementNames.Terminal);


        public void Load(Stage terminal)
        {
            Unit TerminalInputHeight = 35;

            terminal.StyleSheet.Configure(Selector.Create<Stage>(ElementNames.Terminal), terminal =>
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
