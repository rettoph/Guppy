using Guppy.Game.Input.Common;
using Guppy.Core.Messaging.Common;

namespace Guppy.Example.Client.Messages
{
    public class PlaceSandInput(bool active) : Message<PlaceSandInput>, IInput
    {
        public readonly bool Active = active;
    }
}
