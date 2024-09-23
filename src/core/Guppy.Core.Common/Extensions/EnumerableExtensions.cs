using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Exceptions;
using Guppy.Core.Common.Extensions.System.Reflection;
using System.Reflection;

namespace Guppy.Core.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static TDelegate? SequenceDelegates<TSequence, TDelegate>(this IEnumerable<object> items)
            where TSequence : unmanaged, Enum
            where TDelegate : Delegate
        {
            IEnumerable<TDelegate> delegates = items.SelectMany(x => EnumerableExtensions.GetDelegateSequences<TSequence, TDelegate>(x))
                .OrderBy(x => x.sequence)
                .Select(x => x.del);

            if (delegates.Any() == false)
            {
                return default;
            }

            return delegates.Aggregate((d1, d2) => (TDelegate)Delegate.Combine(d1, d2));
        }

        public static T[] Sequence<T, TSequence>(this IEnumerable<object> items)
            where T : notnull
            where TSequence : unmanaged, Enum
        {
            IEnumerable<T> sequenced = items
                .OfType<T>()
                .Select(x => (item: x, sequences: EnumerableExtensions.GetSequences<TSequence>(x.GetType())))
                .SelectMany(itemSequences => itemSequences.sequences.Select(sequence => (itemSequences.item, sequence)))
                .OrderBy(x => x.sequence)
                .Select(x => x.item);

            return sequenced.ToArray();
        }

        private static IEnumerable<TSequence> GetSequences<TSequence>(MemberInfo member)
            where TSequence : unmanaged, Enum
        {
            if (member.TryGetAllCustomAttributes<SequenceAttribute<TSequence>>(true, out var sequenceAttributes))
            {
                return member.GetCustomAttributes<SequenceAttribute<TSequence>>().Select(x => x.Value);
            }

            if (member.TryGetAllCustomAttributes<RequireSequenceAttribute<TSequence>>(true, out var requiredSequenceAttributes))
            {
                throw new SequenceException(typeof(TSequence), member);
            }

            return Enumerable.Empty<TSequence>();
        }

        private static IEnumerable<(TDelegate del, TSequence sequence)> GetDelegateSequences<TSequence, TDelegate>(object instance, params Type[] requiredParameterTypes)
            where TSequence : unmanaged, Enum
            where TDelegate : Delegate
        {
            return instance.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(mi => mi.IsCompatibleWithDelegate<TDelegate>())
                .SelectMany(mi =>
                {
                    TDelegate del = mi.CreateDelegate<TDelegate>(instance);

                    return EnumerableExtensions.GetSequences<TSequence>(mi).Select(sequence => (del, sequence));
                });
        }
    }
}
