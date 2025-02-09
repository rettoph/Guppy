using Guppy.Game.Input.Common;

namespace Guppy.Example.Client.Messages
{
    public class PlaceSandInput(bool active) : InputMessage<PlaceSandInput>
    {
        public readonly bool Active = active;
    }
}