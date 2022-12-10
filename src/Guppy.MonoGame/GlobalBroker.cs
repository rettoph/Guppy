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
    internal sealed class GlobalBroker : IGlobalBroker
    {
        public PublishStrategy Strategy { get; }

        public GlobalBroker(PublishStrategy strategy)
        {
            this.Strategy = strategy;
        }

        public void Publish(in IMessage message)
        {
            this.Strategy.Publish(in message);
        }
    }
}
