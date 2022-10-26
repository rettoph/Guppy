using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public class BusConfiguration
    {
        public Type Type { get; init; }

        public int Queue { get; init; }

        public BusConfiguration(Type type, int queue)
        {
            this.Queue = queue;
            this.Type = type;
        }
    }

    public sealed class BusConfiguration<T> : BusConfiguration
        where T : notnull
    {
        public BusConfiguration(int queue) : base(typeof(T), queue)
        {
        }
    }
}
