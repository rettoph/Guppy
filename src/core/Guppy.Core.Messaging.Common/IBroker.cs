﻿namespace Guppy.Core.Messaging.Common
{
    public interface IBroker<TBase> : IBaseBroker
        where TBase : class, IMessage
    {
        public void Publish(in TBase message);

        void Subscribe(IBaseSubscriber<TBase> subscriber);

        void Unsubscribe(IBaseSubscriber<TBase> subscriber);
    }
}
