using Guppy.Core.Common.Extensions.System.Reflection;
using Guppy.Core.Common.Extensions.Utilities;
using Guppy.Core.Common.Utilities;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Guppy.Core.Common
{
    public abstract class DelegateSequenceGroup<TSequenceGroup, TDelegate>
        where TSequenceGroup : unmanaged, Enum
        where TDelegate : Delegate
    {
        private readonly Type? _delegateType;
        private readonly List<Delegator<TDelegate>> _orphans;
        private readonly HashSet<Delegator<TDelegate>> _all;
        private TDelegate? _sequenced;
        private readonly Dictionary<SequenceGroup<TSequenceGroup>, TDelegate?> _grouped;

        public TDelegate? Sequenced => _sequenced;
        public readonly ReadOnlyDictionary<SequenceGroup<TSequenceGroup>, TDelegate?> Grouped;
        public readonly ReadOnlyCollection<Delegator<TDelegate>> Orphans;

        public DelegateSequenceGroup(Type? delegateType = null)
        {
            _delegateType = delegateType;
            _grouped = [];
            _all = [];
            _orphans = [];

            this.Grouped = new ReadOnlyDictionary<SequenceGroup<TSequenceGroup>, TDelegate?>(_grouped);
            this.Orphans = new ReadOnlyCollection<Delegator<TDelegate>>(_orphans);
        }

        public void Add(IEnumerable<Delegator<TDelegate>> delegators)
        {
            bool modified = false;

            foreach (Delegator<TDelegate> delegator in delegators)
            {
                if (_all.Add(delegator) == false)
                {
                    continue;
                }

                if (delegator.Method.TryGetSequenceGroup<TSequenceGroup>(false, delegator.Target, out var sequenceGroup) == false)
                {
                    _orphans.Add(delegator);
                    continue;
                }

                ref TDelegate? group = ref CollectionsMarshal.GetValueRefOrAddDefault(_grouped, sequenceGroup, out bool exists);
                group = (TDelegate)Delegate.Combine(group, delegator.Delegate);

                modified = true;
            }

            if (modified == false)
            {
                return;
            }

            _sequenced = _all.Sequence<TDelegate, TSequenceGroup>().Combine();
        }

        public void Remove(IEnumerable<Delegator<TDelegate>> delegators)
        {
            bool modified = false;

            foreach (Delegator<TDelegate> delegator in delegators)
            {
                if (_all.Remove(delegator) == false)
                {
                    continue;
                }

                if (_orphans.Remove(delegator) == true)
                {
                    continue;
                }

                if (delegator.Method.TryGetSequenceGroup<TSequenceGroup>(false, delegator.Target, out var sequenceGroup) == true)
                {
                    ref TDelegate? group = ref CollectionsMarshal.GetValueRefOrAddDefault(_grouped, sequenceGroup, out bool exists);
                    group = (TDelegate?)Delegate.Remove(group, delegator.Delegate);
                }

                modified = true;
            }

            if (modified == false)
            {
                return;
            }

            _sequenced = _all.Sequence<TDelegate, TSequenceGroup>().Combine();
        }

        public void Add(IEnumerable<TDelegate> delegates)
        {
            IEnumerable<Delegator<TDelegate>> delegators = delegates.Select(x => new Delegator<TDelegate>(x));
            this.Add(delegators);
        }

        public void Remove(IEnumerable<TDelegate> delegates)
        {
            IEnumerable<Delegator<TDelegate>> delegators = delegates.Select(x => new Delegator<TDelegate>(x));
            this.Remove(delegators);
        }

        public void Add(IEnumerable<object> instances)
        {
            IEnumerable<Delegator<TDelegate>> delegators = instances.SelectMany(x => x.GetMatchingDelegators<TDelegate>(_delegateType));
            this.Add(delegators);
        }

        public void Remove(IEnumerable<object> instances)
        {
            IEnumerable<Delegator<TDelegate>> delegators = instances.SelectMany(x => x.GetMatchingDelegators<TDelegate>(_delegateType));
            this.Remove(delegators);
        }

        public static void Invoke(IEnumerable<Delegator<TDelegate>> delegators, object[] args)
        {
            foreach (Delegator<TDelegate> del in delegators.Sequence<TDelegate, TSequenceGroup>())
            {
                del.Delegate.DynamicInvoke(args);
            }
        }

        public static void Invoke(IEnumerable<TDelegate> delegates, object[] args)
        {
            IEnumerable<Delegator<TDelegate>> delegators = delegates.Select(x => new Delegator<TDelegate>(x));
            DelegateSequenceGroup<TSequenceGroup, TDelegate>.Invoke(delegators, args);
        }

        public static void Invoke(IEnumerable<object> instances, object[] args)
        {
            IEnumerable<Delegator<TDelegate>> delegators = instances.SelectMany(x => x.GetMatchingDelegators<TDelegate>());
            DelegateSequenceGroup<TSequenceGroup, TDelegate>.Invoke(delegators, args);
        }
    }

    public abstract class DelegateSequenceGroup<TSequenceGroup>
        where TSequenceGroup : unmanaged, Enum
    {
        public static void Invoke(IEnumerable<Delegate> delegates, object[] args)
        {
            DelegateSequenceGroup<TSequenceGroup, Delegate>.Invoke(delegates, args);
        }

        public static void Invoke(IEnumerable<object> instances, Type delegateType, object[] args)
        {
            IEnumerable<Delegator<Delegate>> delegators = instances.SelectMany(x => x.GetMatchingDelegators<Delegate>(delegateType));
            DelegateSequenceGroup<TSequenceGroup, Delegate>.Invoke(delegators, args);
        }
    }
}
