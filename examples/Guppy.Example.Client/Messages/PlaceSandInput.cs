using Guppy.Game.Input.Common;
using Guppy.Core.Messaging.Common;

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
