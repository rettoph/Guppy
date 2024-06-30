namespace Guppy.Core.Messaging.Common
{
    public interface IBaseBroker
    {
        bool TrySubscribe(IBaseSubscriber subscriber);
        bool TryUnsubscribe(IBaseSubscriber subscriber);
    }
}
