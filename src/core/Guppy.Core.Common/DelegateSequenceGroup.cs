using Guppy.Core.Common.Extensions.System.Reflection;
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
        private readonly List<Delegator<TDelegate>> _sequenced;
        private readonly Dictionary<SequenceGroup<TSequenceGroup>, List<Delegator<TDelegate>>> _grouped;
        private readonly Dictionary<SequenceGroup<TSequenceGroup>, ReadOnlyCollection<Delegator<TDelegate>>> _readonlyGrouped;

        public readonly ReadOnlyCollection<Delegator<TDelegate>> Sequenced;
        public readonly ReadOnlyDictionary<SequenceGroup<TSequenceGroup>, ReadOnlyCollection<Delegator<TDelegate>>> Grouped;
        public readonly ReadOnlyCollection<Delegator<TDelegate>> Orphans;

        public DelegateSequenceGroup(Type? delegateType = null)
        {
            _delegateType = delegateType;
            _grouped = new Dictionary<SequenceGroup<TSequenceGroup>, List<Delegator<TDelegate>>>();
            _readonlyGrouped = new Dictionary<SequenceGroup<TSequenceGroup>, ReadOnlyCollection<Delegator<TDelegate>>>();
            _all = new HashSet<Delegator<TDelegate>>();
            _sequenced = new List<Delegator<TDelegate>>();
            _orphans = new List<Delegator<TDelegate>>();

            this.Sequenced = new ReadOnlyCollection<Delegator<TDelegate>>(_sequenced);
            this.Grouped = new ReadOnlyDictionary<SequenceGroup<TSequenceGroup>, ReadOnlyCollection<Delegator<TDelegate>>>(_readonlyGrouped);
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

                SequenceGroup<TSequenceGroup>[] sequenceGroups = delegator.Method.GetSequenceGroups<TSequenceGroup>(false).ToArray();
                if (sequenceGroups.Length == 0)
                {
                    _orphans.Add(delegator);
                    continue;
                }

                foreach (SequenceGroup<TSequenceGroup> sequenceGroup in sequenceGroups)
                {
                    this.GetSequenceGroup(sequenceGroup).Add(delegator);
                }

                modified = true;
            }

            if (modified == false)
            {
                return;
            }

            _sequenced.Clear();
            _sequenced.AddRange(_grouped.OrderBy(x => x.Key).SelectMany(x => x.Value));
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

                foreach (SequenceGroup<TSequenceGroup> sequenceGroup in delegator.Method.GetSequenceGroups<TSequenceGroup>(false))
                {
                    this.GetSequenceGroup(sequenceGroup).Remove(delegator);
                }

                modified = true;
            }

            if (modified == false)
            {
                return;
            }

            _sequenced.Clear();
            _sequenced.AddRange(_grouped.OrderBy(x => x.Key).SelectMany(x => x.Value));
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

        private List<Delegator<TDelegate>> GetSequenceGroup(SequenceGroup<TSequenceGroup> sequenceGroup)
        {
            ref List<Delegator<TDelegate>>? delegators = ref CollectionsMarshal.GetValueRefOrAddDefault(_grouped, sequenceGroup, out bool exists);
            if (exists == false)
            {
                delegators = new List<Delegator<TDelegate>>();
                _readonlyGrouped.Add(sequenceGroup, new ReadOnlyCollection<Delegator<TDelegate>>(delegators));
            }

            return delegators!;
        }

        public static void Invoke(IEnumerable<Delegator<TDelegate>> delegators, object[] args)
        {
            foreach (Delegator<TDelegate> del in delegators.OrderBy(x => x.Method.GetSequenceGroup<TSequenceGroup>(false)))
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
