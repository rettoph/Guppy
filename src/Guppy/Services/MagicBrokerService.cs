using Guppy.Common;
using Guppy.Messaging;
using Guppy.Messaging.Extensions;
using Guppy.Messaging.Services;

namespace Guppy.Services
{
    internal sealed class MagicBrokerService : IMagicBrokerService
    {
        private readonly IMagicBroker[] _brokers;

        public MagicBrokerService(IFiltered<IMagicBroker> brokers)
        {
            _brokers = brokers.Instances.ToArray();
        }

        public void Subscribe(IEnumerable<object> subscribers)
        {
            _brokers.SubscribeMany(subscribers);
        }

        public void Unsubscribe(IEnumerable<object> subscribers)
        {
            _brokers.UnsubscribeMany(subscribers);
        }
    }
}
