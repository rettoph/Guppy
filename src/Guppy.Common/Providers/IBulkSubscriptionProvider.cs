namespace Guppy.Common.Providers
{
    public interface IBulkSubscriptionProvider
    {
        void Subscribe(IEnumerable<object> instances);
        void Unsubscribe(IEnumerable<object> instances);
    }
}
