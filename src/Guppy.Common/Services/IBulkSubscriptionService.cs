namespace Guppy.Common.Services
{
    public interface IBulkSubscriptionService
    {
        void Subscribe(IEnumerable<object> instances);
        void Unsubscribe(IEnumerable<object> instances);
    }
}
