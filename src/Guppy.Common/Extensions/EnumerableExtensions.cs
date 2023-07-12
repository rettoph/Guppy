using Guppy.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static T[] Sequence<T, TSequence>(this IEnumerable<T> items, TSequence defaultSequence)
            where TSequence : Enum
        {
            return items.Select(x => (item: x, sequence: EnumerableExtensions.GetSequence<T, TSequence>(x, defaultSequence)))
                .OrderBy(x => x.sequence)
                .Select(x => x.item)
                .ToArray();
        }

        private static TSequence GetSequence<T, TSequence>(T item, TSequence defaultSequence)
            where TSequence : Enum
        {
            if (item is null)
            {
                return defaultSequence;

            }

            if (!item.GetType().HasCustomAttribute<SequenceAttribute<TSequence>>(true))
            {
                return defaultSequence;
            }

            return item.GetType().GetCustomAttribute<SequenceAttribute<TSequence>>()!.Value;
        }
    }
}
