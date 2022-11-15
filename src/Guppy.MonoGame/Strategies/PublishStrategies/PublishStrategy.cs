using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Strategies.PublishStrategies
{
    public abstract class PublishStrategy
    {
        public abstract void Publish(in IMessage message);
    }
}
