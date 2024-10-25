using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions.System;
using Guppy.Core.Common.Helpers;

namespace Guppy.Core.Common
{
    public readonly struct SequenceGroup<T>(string name, int order) : IComparable<SequenceGroup<T>>
        where T : unmanaged, Enum
    {
        public readonly string Name = name;
        public readonly int Order = order;
        private static readonly Dictionary<T, SequenceGroup<T>> _enumMap;

        static SequenceGroup()
        {
            _enumMap = EnumHelper.ToDictionary<T, SequenceGroup<T>>(x =>
            {
                IEnumerable<SequenceGroupAttribute<T>> sequenceGroupAttributes = x.GetCustomAttributes<SequenceGroupAttribute<T>>();
                if (sequenceGroupAttributes.Any() == false)
                {
                    return new SequenceGroup<T>(x.ToString(), (int)(object)x);
                }

                return sequenceGroupAttributes.Single().Value;
            });
        }

        public static SequenceGroup<T> GetByValue(T value)
        {
            return _enumMap[value];
        }

        public readonly int CompareTo(SequenceGroup<T> other)
        {
            return this.Order.CompareTo(other.Order);
        }

        public override readonly bool Equals(object? obj)
        {
            return obj is SequenceGroup<T> group &&
                   Name == group.Name &&
                   Order == group.Order;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(Name, Order);
        }

        public static bool operator <(SequenceGroup<T> left, SequenceGroup<T> right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(SequenceGroup<T> left, SequenceGroup<T> right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(SequenceGroup<T> left, SequenceGroup<T> right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(SequenceGroup<T> left, SequenceGroup<T> right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator ==(SequenceGroup<T> left, SequenceGroup<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SequenceGroup<T> left, SequenceGroup<T> right)
        {
            return !(left == right);
        }
    }
}
