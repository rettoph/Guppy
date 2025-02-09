namespace Guppy.Game.Input.Common.Messages
{
    public class Toggle<T>(T? item) : InputMessage<Toggle<T>>, IInputMessage
    {
        public static readonly Toggle<T> Instance = new(default);

        public readonly T? Item = item;
    }
}