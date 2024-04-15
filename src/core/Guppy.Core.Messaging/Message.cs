namespace Guppy.Core.Messaging
{
    public class Message<T> : IMessage
    {
        public Type Type => typeof(T);
    }
}
