namespace Guppy.Core.Messaging
{
    /// <summary>
    /// A interface that should allow the bulk subscription of
    /// any type to be subscribed to the current broker.
    /// 
    /// Guppy implementations & components will manage the automatic
    /// subscription of all subscribers to any global/scoped components
    /// implementing this type
    /// </summary>
    public interface IMagicBroker
    {
        void Subscribe(IEnumerable<object> instances);
        void Unsubscribe(IEnumerable<object> instances);
    }
}
