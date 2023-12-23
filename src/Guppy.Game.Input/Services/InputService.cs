using Guppy.Common.Providers;
using Guppy.Messaging;

namespace Guppy.Game.Input.Services
{
    internal class InputService : Broker<IInput>, IInputService, IBulkSubscriptionProvider
    {
        void IBulkSubscriptionProvider.Subscribe(IEnumerable<object> instances)
        {
            foreach (IBaseSubscriber<IInput> subscriber in instances.OfType<IBaseSubscriber<IInput>>())
            {
                this.Subscribe(subscriber);
            }
        }

        void IBulkSubscriptionProvider.Unsubscribe(IEnumerable<object> instances)
        {
            foreach (IBaseSubscriber<IInput> subscriber in instances.OfType<IBaseSubscriber<IInput>>())
            {
                this.Unsubscribe(subscriber);
            }
        }
    }
}
