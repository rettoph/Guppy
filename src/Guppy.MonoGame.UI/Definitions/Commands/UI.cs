using Guppy.MonoGame.Definitions;
using Guppy.MonoGame.UI.Messages.Inputs;
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
        internal sealed class Key : CommandDefinition<ImGuiKeyStateInput>
        {
            public override Type? Parent => CommandDefinition.Guppy;

            public Option<ImGuiKey> Which => new Option<ImGuiKey>("--which");
            public Option<ButtonState> State => new Option<ButtonState>("--state");

            public override ImGuiKeyStateInput BindData(BindingContext context)
            {
                return new ImGuiKeyStateInput(
                    key: context.ParseResult.GetValueForOption(this.Which),
                    state: context.ParseResult.GetValueForOption(this.State));
            }
        }
    }
}
