using Guppy.Game.Input.Common;
using Guppy.Core.Messaging.Common;

namespace Guppy.Game.MonoGame.Messages
{
    public class Toggle<T>(T? item) : Message<Toggle<T>>, IInput
    {
        public static readonly Toggle<T> Instance = new(default);

        public readonly T? Item = item;
    }
}
