namespace Guppy.Core.Messaging.Common
{
    public interface IMessagePublisher
    {
        public abstract void Subscribe(object instance);
        public abstract void Subscribe(IEnumerable<object> instances);

        public abstract void Unsubscribe(object instance);
        public abstract void Unsubscribe(IEnumerable<object> instances);
    }
}
