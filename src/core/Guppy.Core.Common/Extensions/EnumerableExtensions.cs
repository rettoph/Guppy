using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Exceptions;
using System.Reflection;

namespace Guppy.Core.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(T, int)> Indices<T>(this IEnumerable<T> items)
        {
            return items.Select((x, i) => (x, i));
        }

        public static T[] Sequence<T, TSequence>(this IEnumerable<object> items, bool strict)
            where T : notnull
            where TSequence : unmanaged, Enum
        {
            IEnumerable<T> sequenced = items
                .OfType<T>()
                .Select(x => (item: x, sequence: EnumerableExtensions.GetSequence<T, TSequence>(x, strict)))
                .Where(x => x.sequence is not null)
                .OrderBy(x => x.sequence)
                .Select(x => x.item);

            return sequenced.ToArray();
        }

        private static TSequence? GetSequence<T, TSequence>(T item, bool strict)
            where TSequence : unmanaged, Enum
        {
            if (item is null)
            {
                if (strict == false)
                {
                    return null;
                }

                throw new SequenceException(typeof(TSequence));
            }

            if (!item.GetType().HasCustomAttribute<SequenceAttribute<TSequence>>(true))
            {
                if (strict == false)
                {
                    return null;
                }

                throw new SequenceException(typeof(TSequence), item);
            }

            return item.GetType().GetCustomAttribute<SequenceAttribute<TSequence>>()!.Value;
        }
    }
}
