using Guppy.MonoGame.Services;
using Guppy.MonoGame.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Definitions
{
    public interface IInputDefinition
    {
        string Key { get; }
        InputSource DefaultSource { get; }

        IInput BuildInput(ICommandService commands);
    }
}
