using Guppy.Game.Input.Common;
using Guppy.Core.Messaging.Common;

namespace Guppy.Game.MonoGame.Messages
{
    public class Toggle<T> : Message<Toggle<T>>, IInput
    {
        public static readonly Toggle<T> Instance = new Toggle<T>(default);

        public readonly T? Item;

        public Toggle(T? item)
        {
            this.Item = item;
        }
    }
}
