namespace Guppy.Core.Messaging.Extensions
{
    public static class IMagicBrokerExtensions
    {
        public static void SubscribeMany(this IEnumerable<IMagicBroker> brokers, IEnumerable<object> subscribers)
        {
            foreach (IMagicBroker broker in brokers)
            {
                broker.Subscribe(subscribers);
            }
        }

        public static void UnsubscribeMany(this IEnumerable<IMagicBroker> brokers, IEnumerable<object> subscribers)
        {
            foreach (IMagicBroker broker in brokers)
            {
                broker.Unsubscribe(subscribers);
            }
        }
    }
}
