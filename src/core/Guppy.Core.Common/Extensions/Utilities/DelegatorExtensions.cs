using Guppy.Core.Common.Extensions.System.Reflection;
using Guppy.Core.Common.Utilities;
using System.Diagnostics;

namespace Guppy.Core.Common.Extensions.Utilities
{
    public static class DelegatorExtensions
    {
        public static IEnumerable<Delegator<TDelegate>> Sequence<TDelegate, TSequenceGroup>(this IEnumerable<Delegator<TDelegate>> delegators)
            where TDelegate : Delegate
            where TSequenceGroup : unmanaged, Enum
        {
            return delegators.Where(x => x.Method.HasSequenceGroup<TSequenceGroup>()).OrderBy(x =>
            {
                if (x.Method.TryGetSequenceGroup<TSequenceGroup>(false, out var sequenceGroup))
                {
                    return sequenceGroup;
                }

                throw new UnreachableException();
            });
        }

        public static TDelegate? Combine<TDelegate>(this IEnumerable<Delegator<TDelegate>> delegators)
            where TDelegate : Delegate
        {
            TDelegate? result = null;

            foreach (var delegator in delegators)
            {
                result = (TDelegate)Delegate.Combine(result, delegator.Delegate);
            }

            return result;
        }
    }
}
