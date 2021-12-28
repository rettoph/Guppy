using Guppy.Attributes;
using Guppy.CommandLine.Interfaces;
using Guppy.IO.Builders;
using Guppy.ServiceLoaders;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Client.ServiceLoaders
{
    public class TestCommand : ICommandData
    {
        public String Data { get; init; }
    }

    [AutoLoad]
    internal class InputCommandServiceLoader : IInputCommandLoader
    {
        public void RegisterInputCommands(InputCommandServiceBuilder inputCommands)
        {
            inputCommands.RegisterInputCommand("test")
                .SetInputButton(Keys.Space)
                .AddCommand(ButtonState.Pressed, new TestCommand()
                {
                    Data = "test"
                });
        }
    }
}
