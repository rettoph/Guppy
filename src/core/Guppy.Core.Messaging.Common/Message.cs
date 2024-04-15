namespace Guppy.Core.Messaging.Common
{
    public class Message<T> : IMessage
    {
        public Type Type => typeof(T);
    }
}
