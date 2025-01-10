using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Guppy.Core.Common.Extensions.System;
using Guppy.Core.Common.Extensions.System.Collections.Generic;
using Guppy.Core.Common.Extensions.System.Reflection;
using Guppy.Core.Common.Extensions.Utilities;
using Guppy.Core.Common.Utilities;

namespace Guppy.Core.Common
{
    public abstract class DelegateSequenceGroup<TSequenceGroup, TDelegate>
        where TSequenceGroup : unmanaged, Enum
        where TDelegate : Delegate
    {
        private readonly Type? _delegateType;
        private readonly List<Delegator<TDelegate>> _orphans;
        private readonly HashSet<Delegator<TDelegate>> _all;
        private readonly Dictionary<SequenceGroup<TSequenceGroup>, TDelegate?> _grouped;

        public TDelegate? Sequenced { get; private set; }
        public readonly ReadOnlyDictionary<SequenceGroup<TSequenceGroup>, TDelegate?> Grouped;
        public readonly ReadOnlyCollection<Delegator<TDelegate>> Orphans;

        public readonly bool Sequence;

        public DelegateSequenceGroup(Type? delegateType, bool sequence)
        {
            this._delegateType = delegateType;
            this._grouped = [];
            this._all = [];
            this._orphans = [];

            this.Grouped = new ReadOnlyDictionary<SequenceGroup<TSequenceGroup>, TDelegate?>(this._grouped);
            this.Orphans = new ReadOnlyCollection<Delegator<TDelegate>>(this._orphans);
            this.Sequence = sequence;
        }

        public void Add(IEnumerable<Delegator<TDelegate>> delegators)
        {
            if (this.Sequence == true)
            { // When the sequence groups are ordered we need to rebuild and resort the entire dictionary when an item is added
                if (this._all.AddRange(delegators) == 0)
                { // No new items added...
                    return;
                }

                // Sequence then sort all items
                this.Sequenced = this._all.OrderBySequenceGroup<TDelegate, TSequenceGroup>(this.Sequence).Combine();

                this._orphans.Clear();
                this._orphans.AddRange(this._all.Where(x => x.Method.HasSequenceGroup<TSequenceGroup>(x.Target) == false));

                this._grouped.Clear();
                var groups = this._all.Except(this._orphans).GroupBy(x => x.Method.GetSequenceGroup<TSequenceGroup>(x.Target));
                foreach (var group in groups)
                {
                    this._grouped.Add(group.Key, group.OrderBy(x => x.Method.GetSequence<TSequenceGroup>(x.Target)).Combine());
                }

                return;
            }

            bool modified = false;

            foreach (Delegator<TDelegate> delegator in delegators)
            {
                if (this._all.Add(delegator) == false)
                {
                    continue;
                }

                if (delegator.Method.TryGetSequenceGroup<TSequenceGroup>(delegator.Target, false, out var sequenceGroup) == false)
                {
                    this._orphans.Add(delegator);
                    continue;
                }

                if (delegator.Method.HasSequence<TSequenceGroup>(delegator.Target) == true)
                {
                    throw new ArgumentException($"{typeof(DelegateSequenceGroup<TSequenceGroup>).GetFormattedName()}::{nameof(Add)} - Method {delegator.Method.Name} should not be ordered.", nameof(delegators));
                }

                ref TDelegate? group = ref CollectionsMarshal.GetValueRefOrAddDefault(this._grouped, sequenceGroup, out bool exists);
                group = (TDelegate)Delegate.Combine(group, delegator.Delegate);

                modified = true;
            }

            if (modified == false)
            {
                return;
            }

            this.Sequenced = this._all.OrderBySequenceGroup<TDelegate, TSequenceGroup>(this.Sequence).Combine();
        }

        public void Remove(IEnumerable<Delegator<TDelegate>> delegators)
        {
            foreach (Delegator<TDelegate> delegator in delegators)
            {
                if (this._all.Remove(delegator) == false)
                {
                    continue;
                }

                if (this._orphans.Remove(delegator) == true)
                {
                    continue;
                }

                if (delegator.Method.TryGetSequenceGroup<TSequenceGroup>(delegator.Target, false, out var sequenceGroup) == true)
                {
                    ref TDelegate? group = ref CollectionsMarshal.GetValueRefOrAddDefault(this._grouped, sequenceGroup, out _);
                    group = (TDelegate?)Delegate.Remove(group, delegator.Delegate);
                }
            }
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
            IEnumerable<Delegator<TDelegate>> delegators = instances.SelectMany(x => x.GetMatchingDelegators<TDelegate>(this._delegateType));
            this.Add(delegators);
        }

        public void Remove(IEnumerable<object> instances)
        {
            IEnumerable<Delegator<TDelegate>> delegators = instances.SelectMany(x => x.GetMatchingDelegators<TDelegate>(this._delegateType));
            this.Remove(delegators);
        }

        /// <summary>
        /// Sequence then invoke all delegators.
        /// </summary>
        /// <param name="delegators"></param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="args"></param>
        public static void Invoke(IEnumerable<Delegator<TDelegate>> delegators, bool sequence, object[] args)
        {
            foreach (Delegator<TDelegate> del in delegators.OrderBySequenceGroup<TDelegate, TSequenceGroup>(sequence))
            {
                del.Delegate.DynamicInvoke(args);
            }
        }

        /// <summary>
        /// Sequence then invoke all delegates.
        /// </summary>
        /// <param name="delegates"></param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="args"></param>
        public static void Invoke(IEnumerable<TDelegate> delegates, bool sequence, object[] args)
        {
            IEnumerable<Delegator<TDelegate>> delegators = delegates.Select(x => new Delegator<TDelegate>(x));
            DelegateSequenceGroup<TSequenceGroup, TDelegate>.Invoke(delegators, sequence, args);
        }

        /// <summary>
        /// Sequence then invoke all matching delegates within a collection of instances.
        /// </summary>
        /// <param name="instances"></param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="args"></param>
        public static void Invoke(IEnumerable<object> instances, bool sequence, object[] args)
        {
            IEnumerable<Delegator<TDelegate>> delegators = instances.SelectMany(x => x.GetMatchingDelegators<TDelegate>());
            DelegateSequenceGroup<TSequenceGroup, TDelegate>.Invoke(delegators, sequence, args);
        }
    }

    public abstract class DelegateSequenceGroup<TSequenceGroup>
        where TSequenceGroup : unmanaged, Enum
    {
        /// <summary>
        /// Sequence then invoke all delegates.
        /// </summary>
        /// <param name="delegates"></param>
        /// <param name="sequenced">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="args"></param>
        public static void Invoke(IEnumerable<Delegate> delegates, bool sequenced, object[] args) => DelegateSequenceGroup<TSequenceGroup, Delegate>.Invoke(delegates, sequenced, args);

        /// <summary>
        /// Sequence then invoke all matching delegates within a collection of instances.
        /// </summary>
        /// <param name="instances"></param>
        /// <param name="delegateType"></param>
        /// <param name="sequenced">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="args"></param>
        public static void Invoke(IEnumerable<object> instances, Type delegateType, bool sequenced, object[] args)
        {
            IEnumerable<Delegator<Delegate>> delegators = instances.SelectMany(x => x.GetMatchingDelegators<Delegate>(delegateType));
            DelegateSequenceGroup<TSequenceGroup, Delegate>.Invoke(delegators, sequenced, args);
        }
    }
}