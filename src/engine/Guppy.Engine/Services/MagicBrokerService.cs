using Guppy.Core.Common;
using Guppy.Core.Messaging;
using Guppy.Core.Messaging.Extensions;
using Guppy.Core.Messaging.Services;

namespace Guppy.Engine.Services
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
