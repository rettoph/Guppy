using Guppy.Common.Attributes;
using System.Reflection;

namespace Guppy.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static T[] Sequence<T, TSequence>(this IEnumerable<T> items, TSequence defaultSequence)
            where TSequence : unmanaged, Enum
        {
            return items.Select(x => (item: x, sequence: EnumerableExtensions.GetSequence<T, TSequence>(x, defaultSequence)))
                .OrderBy(x => x.sequence)
                .Select(x => x.item)
                .ToArray();
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
