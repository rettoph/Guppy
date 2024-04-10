using Guppy.Common.Attributes;
using System.Reflection;

namespace Guppy.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(T, int)> Indices<T>(this IEnumerable<T> items)
        {
            return items.Select((x, i) => (x, i));
        }

        public static T[] Sequence<T, TSequence>(this IEnumerable<T> items, TSequence defaultSequence, bool reverse = false)
            where TSequence : unmanaged, Enum
        {
            IEnumerable<T> sequenced = items.Select(x => (item: x, sequence: EnumerableExtensions.GetSequence<T, TSequence>(x, defaultSequence)))
                .OrderBy(x => x.sequence)
                .Select(x => x.item);

            if (reverse)
            {
                sequenced = sequenced.Reverse();
            }

            return sequenced.ToArray();
        }

        private static TSequence GetSequence<T, TSequence>(T item, TSequence defaultSequence)
            where TSequence : unmanaged, Enum
        {
            if (item is null)
            {
                return defaultSequence;

            }

            if (item is ISequenceable<TSequence> sequenceable && sequenceable.Sequence is not null)
            {
                return sequenceable.Sequence.Value;
            }

            if (!item.GetType().HasCustomAttribute<SequenceAttribute<TSequence>>(true))
            {
                return defaultSequence;
            }

            return item.GetType().GetCustomAttribute<SequenceAttribute<TSequence>>()!.Value;
        }
    }
}
