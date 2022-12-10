using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public class BusConfiguration
    {
        internal sealed class TypeQueueConfiguration
        {
            public required Type Type { get; init; }

            public required int Queue { get; init; }
        }

        internal IList<TypeQueueConfiguration> TypeQueues { get; }

        public BusConfiguration()
        {
            this.TypeQueues = new List<TypeQueueConfiguration>();
        }

        public BusConfiguration SetTypeQueue(Type type, int queue)
        {
            ThrowIf.Type.IsNotAssignableFrom<IMessage>(type);

            this.TypeQueues.Add(new TypeQueueConfiguration()
            {
                Type = type,
                Queue = queue
            });

            return this;
        }

        public BusConfiguration SetTypeQueue<T>(int queue)
            where T : IMessage
        {
            return this.SetTypeQueue(typeof(T), queue);
        }
    }
}
