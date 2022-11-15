using Guppy.Common;
using Guppy.MonoGame.Strategies.PublishStrategies;
using Guppy.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    internal sealed class GlobalBroker<TPublishStrategy> : IGlobalBroker
        where TPublishStrategy : PublishStrategy
    {
        private readonly TPublishStrategy _strategy;

        public PublishStrategy PublishStrategy => _strategy;

        public GlobalBroker(TPublishStrategy strategy, IServiceProvider provider)
        {
            _strategy = strategy;
        }

        public void Publish(in IMessage message)
        {
            _strategy.Publish(in message);
        }
    }
}
