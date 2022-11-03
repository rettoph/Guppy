using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Implementations
{
    public abstract class BusPublisher : IBusPublisher
    {
        private readonly HashSet<IBus> _busses;

        public BusPublisher(IEnumerable<IBus> busses)
        {
            _busses = new HashSet<IBus>(busses);
        }

        public BusPublisher()
        {
            _busses = new HashSet<IBus>();
        }

        public virtual void Dispose()
        {
            _busses.Clear();
        }

        public void Subscribe(IBus bus)
        {
            _busses.Add(bus);
        }

        public void Unsubscribe(IBus bus)
        {
            _busses.Remove(bus);
        }

        protected virtual void Publish(in IMessage message)
        {
            foreach(IBus bus in _busses)
            {
                bus.Publish(message);
            }
        }
    }
}
