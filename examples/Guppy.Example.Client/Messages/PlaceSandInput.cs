using Guppy.Core.Messaging.Common;
using Guppy.Game.Input.Common;

namespace Guppy.Example.Client.Messages
{
    public class PlaceSandInput(bool active) : Message<PlaceSandInput>, IInput
    {
        public readonly bool Active = active;
    }
}