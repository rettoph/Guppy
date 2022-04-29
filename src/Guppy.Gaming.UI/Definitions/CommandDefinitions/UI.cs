using Guppy.Gaming.Definitions;
using Guppy.Gaming.UI.Structs;
using Guppy.Threading;
using ImGuiNET;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.UI.Definitions.CommandDefinitions
{
    internal sealed class UI : CommandDefinition
    {
        public override ICommandHandler? Publisher(IBroker broker)
        {
            return null;
        }

        internal sealed class Key : CommandDefinition
        {
            public override Type? Parent => CommandDefinition.Guppy;

            public override Option[] Options => new Option[]
            {
            new Option<ImGuiKey>("--which"),
            new Option<ButtonState>("--state")
            };

            public override ICommandHandler? Publisher(IBroker broker)
            {
                return CommandHandler.Create<ImGuiKey, ButtonState>((which, state) =>
                {
                    broker.Publish<ImGuiKeyState>(new ImGuiKeyState(which, state));
                });
            }
        }
    }
}
