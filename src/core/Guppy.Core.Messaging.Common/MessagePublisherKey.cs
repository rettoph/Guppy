namespace Guppy.Core.Messaging.Common
{
    public readonly struct MessagePublisherKey(int value)
    {
        private readonly int _value = value;

        public static class Instance<TSequenceGroup, TId, TMessage>
            where TSequenceGroup : unmanaged, Enum
        {
            public static readonly MessagePublisherKey Value = MessagePublisherKey.Create<TSequenceGroup, TId, TMessage>();
        }

        public static class Instance<TSequenceGroup, TMessage>
            where TSequenceGroup : unmanaged, Enum
        {
            private class IdPlaceholder { }
            public static readonly MessagePublisherKey Value = MessagePublisherKey.Create<TSequenceGroup, IdPlaceholder, TMessage>();
        }

        public static MessagePublisherKey Create<TSequenceGroup, TId, TMessage>()
            where TSequenceGroup : unmanaged, Enum
        {
            return new MessagePublisherKey(HashCode.Combine(typeof(TSequenceGroup).GetHashCode(), typeof(TId).GetHashCode(), typeof(TMessage).GetHashCode()));
        }

        public override bool Equals(object? obj)
        {
            return obj is MessagePublisherKey key &&
                   this._value == key._value;
        }

        public override int GetHashCode()
        {
            return this._value;
        }

        public static bool operator ==(MessagePublisherKey left, MessagePublisherKey right)
        {
            return left._value == right._value;
        }

        public static bool operator !=(MessagePublisherKey left, MessagePublisherKey right)
        {
            return left._value != right._value;
        }
    }
}
