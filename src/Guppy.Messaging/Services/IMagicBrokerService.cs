namespace Guppy.Messaging.Services
{
    public interface IMagicBrokerService
    {
        void Subscribe(IEnumerable<object> subscribers);
        void Unsubscribe(IEnumerable<object> subscribers);
    }
}
