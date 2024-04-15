using Guppy.Game.Input;
using Guppy.Core.Messaging;

namespace Guppy.Example.Client.Messages
{
    public class PlaceSandInput : Message<PlaceSandInput>, IInput
    {
        public readonly bool Active;

        public PlaceSandInput(bool active)
        {
            Active = active;
        }
    }
}
