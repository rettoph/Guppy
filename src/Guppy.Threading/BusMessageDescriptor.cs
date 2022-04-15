using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading
{
    public abstract class BusMessageDescriptor
    {
        public readonly Type Type;
        public readonly int Queue;

        protected BusMessageDescriptor(Type type, int queue)
        {
            this.Type = type;
            this.Queue = queue;
        }

        public static BusMessageDescriptor Create<T>(int queue)
        {
            return new BusMessageDescriptor<T>(queue);
        }
    }

    internal sealed class BusMessageDescriptor<T> : BusMessageDescriptor
    {
        internal BusMessageDescriptor(int queue) : base(typeof(T), queue)
        {
        }
    }
}
