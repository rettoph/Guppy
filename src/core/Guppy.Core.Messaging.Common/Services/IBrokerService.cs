namespace Guppy.Core.Messaging.Common.Services
{
    public interface IBrokerService
    {
        IEnumerable<IBaseBroker> GetAll();

        void Add(IBaseBroker broker);
        void Remove(IBaseBroker broker);

        void AddSubscribers(IEnumerable<IBaseSubscriber> subscribers);
        void RemoveSubscribers(IEnumerable<IBaseSubscriber> subscribers);

        /// <summary>
        /// Automatically add any filtered subscribers within the current service scope
        /// implementing the given type
        /// </summary>
        /// <typeparam name="TBrokers"></typeparam>
        void AddSubscribers<TSubscribers>() where TSubscribers : class;

        /// <summary>
        /// Automatically remove any filtered subscribers within the current service scope
        /// implementing the given type
        /// </summary>
        /// <typeparam name="TBrokers"></typeparam>
        void RemoveSubscribers<TSubscribers>() where TSubscribers : class;
    }
}
