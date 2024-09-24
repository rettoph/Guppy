﻿using Guppy.Core.Common.Extensions.System.Reflection;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Guppy.Core.Common
{
    /// <summary>
    /// Manages a collection of sequenced and grouped actions.
    /// 
    /// These are methods with an attached <see cref="Attributes.SequenceGroupAttribute{TSequenceGroup}"/> attributes.
    /// 
    /// When adding an <see cref="object"/> instance, ALL public methods matching the action signature and having
    /// the required attribute will be added,
    /// </summary>
    /// <typeparam name="TSequenceGroup"></typeparam>
    /// <typeparam name="TAction"></typeparam>
    public class ActionSequenceGroupBase<TSequenceGroup, TAction>
        where TSequenceGroup : unmanaged, Enum
        where TAction : Delegate
    {
        private readonly HashSet<TAction> _all;
        private readonly List<TAction> _sequenced;
        private readonly Dictionary<SequenceGroup<TSequenceGroup>, List<TAction>> _grouped;
        private readonly Dictionary<SequenceGroup<TSequenceGroup>, ReadOnlyCollection<TAction>> _readonlyGrouped;

        public readonly ReadOnlyCollection<TAction> Sequenced;
        public readonly ReadOnlyDictionary<SequenceGroup<TSequenceGroup>, ReadOnlyCollection<TAction>> Grouped;

        public ActionSequenceGroupBase()
        {
            _grouped = new Dictionary<SequenceGroup<TSequenceGroup>, List<TAction>>();
            _readonlyGrouped = new Dictionary<SequenceGroup<TSequenceGroup>, ReadOnlyCollection<TAction>>();
            _all = new HashSet<TAction>();
            _sequenced = new List<TAction>();

            this.Sequenced = new ReadOnlyCollection<TAction>(_sequenced);
            this.Grouped = new ReadOnlyDictionary<SequenceGroup<TSequenceGroup>, ReadOnlyCollection<TAction>>(_readonlyGrouped);
        }

        public void Add(IEnumerable<TAction> actions)
        {
            bool modified = false;

            foreach (TAction action in actions)
            {
                if (_all.Add(action) == false)
                {
                    continue;
                }

                foreach (SequenceGroup<TSequenceGroup> sequenceGroup in action.GetMethodInfo().GetSequenceGroups<TSequenceGroup>(false))
                {
                    this.GetSequenceGroup(sequenceGroup).Add(action);
                    modified = true;
                }
            }

            if (modified == false)
            {
                return;
            }

            _sequenced.Clear();
            _sequenced.AddRange(_grouped.OrderBy(x => x.Key).SelectMany(x => x.Value));
        }

        public void Remove(IEnumerable<TAction> actions)
        {
            bool modified = false;

            foreach (TAction action in actions)
            {
                if (_all.Remove(action) == false)
                {
                    continue;
                }

                foreach (SequenceGroup<TSequenceGroup> sequenceGroup in action.GetMethodInfo().GetSequenceGroups<TSequenceGroup>(false))
                {
                    this.GetSequenceGroup(sequenceGroup).Remove(action);
                    modified = true;
                }
            }

            if (modified == false)
            {
                return;
            }

            _sequenced.Clear();
            _sequenced.AddRange(_grouped.OrderBy(x => x.Key).SelectMany(x => x.Value));
        }

        public void Add(IEnumerable<object> instances)
        {
            IEnumerable<TAction> actions = instances.SelectMany(x => x.GetMatchingDelegates<TAction>());
            this.Add(actions);
        }

        public void Remove(IEnumerable<object> instances)
        {
            IEnumerable<TAction> actions = instances.SelectMany(x => x.GetMatchingDelegates<TAction>());
            this.Remove(actions);
        }

        private List<TAction> GetSequenceGroup(SequenceGroup<TSequenceGroup> sequenceGroup)
        {
            ref List<TAction>? actions = ref CollectionsMarshal.GetValueRefOrAddDefault(_grouped, sequenceGroup, out bool exists);
            if (exists == false)
            {
                actions = new List<TAction>();
                _readonlyGrouped.Add(sequenceGroup, new ReadOnlyCollection<TAction>(actions));
            }

            return actions!;
        }
    }

    public sealed class ActionSequenceGroup<TSequenceGroup, TParam> : ActionSequenceGroupBase<TSequenceGroup, Action<TParam>>
        where TSequenceGroup : unmanaged, Enum
    {
        public ActionSequenceGroup() : base()
        {
        }
        public ActionSequenceGroup(IEnumerable<Action<TParam>> actions) : base()
        {
            this.Add(actions);
        }
        public ActionSequenceGroup(IEnumerable<object> instances) : base()
        {
            this.Add(instances);
        }

        public void Invoke(TParam param)
        {
            foreach (Action<TParam> action in this.Sequenced)
            {
                action(param);
            }
        }

        public void Invoke(SequenceGroup<TSequenceGroup> sequenceGroup, TParam param)
        {
            foreach (Action<TParam> action in this.Grouped[sequenceGroup])
            {
                action(param);
            }
        }
    }
}