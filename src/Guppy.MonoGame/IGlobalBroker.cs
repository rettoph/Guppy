using Guppy.Common;
using Guppy.MonoGame.Strategies.PublishStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    public interface IGlobalBroker
    {
        PublishStrategy Strategy { get; }
        public abstract void Publish(in IMessage message);
    }
}
