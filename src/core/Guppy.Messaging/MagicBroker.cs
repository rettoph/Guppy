
namespace Guppy.Messaging
{
    public abstract class MagicBroker<TBase> : Broker<TBase>, IMagicBroker
        where TBase : class, IMessage
    {
        public void Subscribe(IEnumerable<object> instances)
        {
            foreach (IBaseSubscriber<TBase> subscriber in instances.OfType<IBaseSubscriber<TBase>>())
            {
                this.Subscribe(subscriber);
            }
        }

        public void Unsubscribe(IEnumerable<object> instances)
        {
            foreach (IBaseSubscriber<TBase> subscriber in instances.OfType<IBaseSubscriber<TBase>>())
            {
                this.Unsubscribe(subscriber);
            }
        }
    }
}
