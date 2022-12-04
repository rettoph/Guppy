using Guppy.MonoGame.Definitions;
using Guppy.MonoGame.UI.Messages;
using ImGuiNET;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Definitions.CommandDefinitions
{
    internal sealed class UI : CommandDefinition
    {
        internal sealed class Key : CommandDefinition<ImGuiKeyEvent>
        {
            public override Type? Parent => CommandDefinition.Guppy;

            public Option<ImGuiKey> Which { get; } = new Option<ImGuiKey>("--which");
            public Option<bool> Down { get; } = new Option<bool>("--down");

            public override ImGuiKeyEvent BindData(BindingContext context)
            {
                return new ImGuiKeyEvent(
                    key: context.ParseResult.GetValueForOption(this.Which),
                    down: context.ParseResult.GetValueForOption(this.Down));
            }
        }
    }
}
