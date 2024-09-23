using Guppy.Core.Common.Extensions.System.Reflection;

namespace Guppy.Core.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static T[] Sequence<T, TSequenceGroup>(
            this IEnumerable<object> items,
            bool strict = true,
            TSequenceGroup? defaultSequenceGroup = null)
                where T : notnull
                where TSequenceGroup : unmanaged, Enum
        {
            IEnumerable<T> sequenced = items
                .OfType<T>()
                .Select(x => (item: x, sequences: x.GetType().GetSequenceGroups<TSequenceGroup>(strict, defaultSequenceGroup)))
                .SelectMany(itemSequences => itemSequences.sequences.Select(sequence => (itemSequences.item, sequence)))
                .OrderBy(x => x.sequence)
                .Select(x => x.item);

            return sequenced.ToArray();
        }
    }
}
