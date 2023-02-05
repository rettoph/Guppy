using Guppy.Attributes;
using Guppy.MonoGame.Structs;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.MonoGame.Definitions
{
    [Service<IInputDefinition>(ServiceLifetime.Singleton, true)]
    public interface IInputDefinition
    {
        string Key { get; }
        InputSource DefaultSource { get; }

        IInput BuildInput();
    }
}
