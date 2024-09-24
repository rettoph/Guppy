﻿using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions.System;
using Guppy.Core.Common.Helpers;

namespace Guppy.Core.Common
{
    public struct SequenceGroup<T> : IComparable<SequenceGroup<T>>
        where T : unmanaged, Enum
    {
        public readonly string Name;
        public readonly int Sequence;

        public SequenceGroup(string name, int sequence)
        {
            this.Name = name;
            this.Sequence = sequence;
        }

        private static Dictionary<T, SequenceGroup<T>> _enumMap;

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

        public int CompareTo(SequenceGroup<T> other)
        {
            return this.Sequence.CompareTo(other.Sequence);
        }

        public override bool Equals(object? obj)
        {
            return obj is SequenceGroup<T> group &&
                   Name == group.Name &&
                   Sequence == group.Sequence;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Sequence);
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