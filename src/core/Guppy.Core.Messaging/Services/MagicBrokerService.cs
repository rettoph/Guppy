using Guppy.Core.Common;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Extensions;
using Guppy.Core.Messaging.Common.Services;

namespace Guppy.Core.Messaging.Services
{
    internal sealed class MagicBrokerService : IMagicBrokerService
    {
        private readonly IFiltered<IMagicBroker> _brokers;

        public MagicBrokerService(IFiltered<IMagicBroker> brokers)
        {
            _brokers = brokers;
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
