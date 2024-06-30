namespace Guppy.Core.Messaging.Common.Extensions
{
    public static class IBrokerExtensions
    {
        public static void TrySubscribeMany(this IBaseBroker broker, IEnumerable<IBaseSubscriber> subscribers)
        {
            foreach (IBaseSubscriber subscriber in subscribers)
            {
                broker.TrySubscribe(subscriber);
            }
        }

        public static void TryUnsubscribeMany(this IBaseBroker broker, IEnumerable<IBaseSubscriber> subscribers)
        {
            foreach (IBaseSubscriber subscriber in subscribers)
            {
                broker.TryUnsubscribe(subscriber);
            }
        }
    }
}
