using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Implementations
{
    public abstract class BusPublisher<T> : IBusPublisher<T>
        where T : notnull, IMessage
    {
        private readonly HashSet<IBus<T>> _busses;

        public BusPublisher(IEnumerable<IBus<T>> busses)
        {
            _busses = new HashSet<IBus<T>>(busses);
        }

        public BusPublisher()
        {
            _busses = new HashSet<IBus<T>>();
        }

        public void Dispose()
        {
            _busses.Clear();
        }

        public void Subscribe(IBus<T> bus)
        {
            _busses.Add(bus);
        }

        public void Unsubscribe(IBus<T> bus)
        {
            _busses.Remove(bus);
        }

        protected virtual void Publish(T message)
        {
            foreach(IBus<T> bus in _busses)
            {
                bus.Publish(message);
            }
        }
    }

    public abstract class BusPublisher : BusPublisher<IMessage>
    {
        public BusPublisher() : base()
        {

        }
        public BusPublisher(IEnumerable<IBus> busses) : base(busses)
        {
        }
    }
}
