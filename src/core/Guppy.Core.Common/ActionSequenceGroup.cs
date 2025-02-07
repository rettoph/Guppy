using Guppy.Core.Common.Extensions.Utilities;
using Guppy.Core.Common.Utilities;

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
    public sealed class ActionSequenceGroup<TSequenceGroup> : DelegateSequenceGroup<TSequenceGroup, Action>
        where TSequenceGroup : unmanaged, Enum
    {
        public ActionSequenceGroup(bool sequence) : base(typeof(Action), sequence)
        {
        }
        public ActionSequenceGroup(bool sequence, IEnumerable<Action> actions) : base(typeof(Action), sequence)
        {
            this.Add(actions);
        }
        public ActionSequenceGroup(bool sequence, IEnumerable<object> instances) : base(typeof(Action), sequence)
        {
            this.Add(instances);
        }

        public void Invoke()
        {
            this.Sequenced?.Invoke();
        }

        public void Invoke(SequenceGroup<TSequenceGroup> sequenceGroup)
        {
            this.Grouped[sequenceGroup]?.Invoke();
        }

        /// <summary>
        /// Sequence then invoke all delegators.
        /// </summary>
        /// <param name="delegators">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        public static void Invoke(IEnumerable<Delegator<Action>> delegators, bool sequence)
        {
            foreach (Delegator<Action> delegator in delegators.OrderBySequenceGroup<Action, TSequenceGroup>(sequence))
            {
                delegator.Delegate();
            }
        }

        /// <summary>
        /// Sequence then invoke all actions.
        /// </summary>
        /// <param name="actions">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        public static void Invoke(IEnumerable<Action> actions, bool sequence)
        {
            IEnumerable<Delegator<Action>> delegators = actions.Select(x => new Delegator<Action>(x));
            ActionSequenceGroup<TSequenceGroup>.Invoke(delegators, sequence);
        }

        /// <summary>
        /// Sequence then invoke all matching delegates within a collection of instances.
        /// </summary>
        /// <param name="instances">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        public static void Invoke(IEnumerable<object> instances, bool sequence)
        {
            IEnumerable<Delegator<Action>> delegators = instances.SelectMany(x => x.GetMatchingDelegators<Action>());
            ActionSequenceGroup<TSequenceGroup>.Invoke(delegators, sequence);
        }
    }

    /// <summary>
    /// Manages a collection of sequenced and grouped actions.
    /// 
    /// These are methods with an attached <see cref="Attributes.SequenceGroupAttribute{TSequenceGroup}"/> attributes.
    /// 
    /// When adding an <see cref="object"/> instance, ALL public methods matching the action signature and having
    /// the required attribute will be added,
    /// </summary>
    /// <typeparam name="TSequenceGroup"></typeparam>
    /// <typeparam name="TParam"></typeparam>
    public sealed class ActionSequenceGroup<TSequenceGroup, TParam> : DelegateSequenceGroup<TSequenceGroup, Action<TParam>>
        where TSequenceGroup : unmanaged, Enum
    {
        public ActionSequenceGroup(bool sequence) : base(typeof(Action<TParam>), sequence)
        {
        }
        public ActionSequenceGroup(bool sequence, IEnumerable<Action<TParam>> actions) : base(typeof(Action<TParam>), sequence)
        {
            this.Add(actions);
        }
        public ActionSequenceGroup(bool sequence, IEnumerable<object> instances) : base(typeof(Action<TParam>), sequence)
        {
            this.Add(instances);
        }

        public void Invoke(TParam param)
        {
            this.Sequenced?.Invoke(param);
        }

        public void Invoke(SequenceGroup<TSequenceGroup> sequenceGroup, TParam param)
        {
            this.Grouped[sequenceGroup]?.Invoke(param);
        }

        /// <summary>
        /// Sequence then invoke all delegators.
        /// </summary>
        /// <param name="delegators">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="param">Parameter to pass when invoking items</param>
        public static void Invoke(IEnumerable<Delegator<Action<TParam>>> delegators, bool sequence, TParam param)
        {
            foreach (Delegator<Action<TParam>> delegator in delegators.OrderBySequenceGroup<Action<TParam>, TSequenceGroup>(sequence))
            {
                delegator.Delegate(param);
            }
        }

        /// <summary>
        /// Sequence then invoke all actions.
        /// </summary>
        /// <param name="actions">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="param">Parameter to pass when invoking items</param>
        public static void Invoke(IEnumerable<Action<TParam>> actions, bool sequence, TParam param)
        {
            IEnumerable<Delegator<Action<TParam>>> delegators = actions.Select(x => new Delegator<Action<TParam>>(x));
            ActionSequenceGroup<TSequenceGroup, TParam>.Invoke(delegators, sequence, param);
        }

        /// <summary>
        /// Sequence then invoke all matching delegates within a collection of instances.
        /// </summary>
        /// <param name="instances">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="param">Parameter to pass when invoking items</param>
        public static void Invoke(IEnumerable<object> instances, bool sequence, TParam param)
        {
            IEnumerable<Delegator<Action<TParam>>> delegators = instances.SelectMany(x => x.GetMatchingDelegators<Action<TParam>>());
            ActionSequenceGroup<TSequenceGroup, TParam>.Invoke(delegators, sequence, param);
        }
    }

    /// <summary>
    /// Manages a collection of sequenced and grouped actions.
    /// 
    /// These are methods with an attached <see cref="Attributes.SequenceGroupAttribute{TSequenceGroup}"/> attributes.
    /// 
    /// When adding an <see cref="object"/> instance, ALL public methods matching the action signature and having
    /// the required attribute will be added,
    /// </summary>
    /// <typeparam name="TSequenceGroup"></typeparam>
    /// <typeparam name="TParam1"></typeparam>
    /// <typeparam name="TParam2"></typeparam>
    public sealed class ActionSequenceGroup<TSequenceGroup, TParam1, TParam2> : DelegateSequenceGroup<TSequenceGroup, Action<TParam1, TParam2>>
        where TSequenceGroup : unmanaged, Enum
    {
        public ActionSequenceGroup(bool sequence) : base(typeof(Action<TParam1, TParam2>), sequence)
        {
        }
        public ActionSequenceGroup(bool sequence, IEnumerable<Action<TParam1, TParam2>> actions) : base(typeof(Action<TParam1, TParam2>), sequence)
        {
            this.Add(actions);
        }
        public ActionSequenceGroup(bool sequence, IEnumerable<object> instances) : base(typeof(Action<TParam1, TParam2>), sequence)
        {
            this.Add(instances);
        }

        public void Invoke(TParam1 param1, TParam2 param2)
        {
            this.Sequenced?.Invoke(param1, param2);
        }

        public void Invoke(SequenceGroup<TSequenceGroup> sequenceGroup, TParam1 param1, TParam2 param2)
        {
            this.Grouped[sequenceGroup]?.Invoke(param1, param2);
        }

        /// <summary>
        /// Sequence then invoke all delegators.
        /// </summary>
        /// <param name="delegators">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="param1">Parameter to pass when invoking items</param>
        /// <param name="param2">Parameter to pass when invoking items</param>
        public static void Invoke(IEnumerable<Delegator<Action<TParam1, TParam2>>> delegators, bool sequence, TParam1 param1, TParam2 param2)
        {
            foreach (Delegator<Action<TParam1, TParam2>> delegator in delegators.OrderBySequenceGroup<Action<TParam1, TParam2>, TSequenceGroup>(sequence))
            {
                delegator.Delegate(param1, param2);
            }
        }

        /// <summary>
        /// Sequence then invoke all actions.
        /// </summary>
        /// <param name="actions">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="param1">Parameter to pass when invoking items</param>
        /// <param name="param2">Parameter to pass when invoking items</param>
        public static void Invoke(IEnumerable<Action<TParam1, TParam2>> actions, bool sequence, TParam1 param1, TParam2 param2)
        {
            IEnumerable<Delegator<Action<TParam1, TParam2>>> delegators = actions.Select(x => new Delegator<Action<TParam1, TParam2>>(x));
            ActionSequenceGroup<TSequenceGroup, TParam1, TParam2>.Invoke(delegators, sequence, param1, param2);
        }

        /// <summary>
        /// Sequence then invoke all matching delegates within a collection of instances.
        /// </summary>
        /// <param name="instances">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="param1">Parameter to pass when invoking items</param>
        /// <param name="param2">Parameter to pass when invoking items</param>
        public static void Invoke(IEnumerable<object> instances, bool sequence, TParam1 param1, TParam2 param2)
        {
            IEnumerable<Delegator<Action<TParam1, TParam2>>> delegators = instances.SelectMany(x => x.GetMatchingDelegators<Action<TParam1, TParam2>>());
            ActionSequenceGroup<TSequenceGroup, TParam1, TParam2>.Invoke(delegators, sequence, param1, param2);
        }
    }

    /// <summary>
    /// Manages a collection of sequenced and grouped actions.
    /// 
    /// These are methods with an attached <see cref="Attributes.SequenceGroupAttribute{TSequenceGroup}"/> attributes.
    /// 
    /// When adding an <see cref="object"/> instance, ALL public methods matching the action signature and having
    /// the required attribute will be added,
    /// </summary>
    /// <typeparam name="TSequenceGroup"></typeparam>
    /// <typeparam name="TParam1"></typeparam>
    /// <typeparam name="TParam2"></typeparam>
    /// <typeparam name="TParam3"></typeparam>
    public sealed class ActionSequenceGroup<TSequenceGroup, TParam1, TParam2, TParam3> : DelegateSequenceGroup<TSequenceGroup, Action<TParam1, TParam2, TParam3>>
        where TSequenceGroup : unmanaged, Enum
    {
        public ActionSequenceGroup(bool sequence) : base(typeof(Action<TParam1, TParam2, TParam3>), sequence)
        {
        }
        public ActionSequenceGroup(bool sequence, IEnumerable<Action<TParam1, TParam2, TParam3>> actions) : base(typeof(Action<TParam1, TParam2, TParam3>), sequence)
        {
            this.Add(actions);
        }
        public ActionSequenceGroup(bool sequence, IEnumerable<object> instances) : base(typeof(Action<TParam1, TParam2, TParam3>), sequence)
        {
            this.Add(instances);
        }

        public void Invoke(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            this.Sequenced?.Invoke(param1, param2, param3);
        }

        public void Invoke(SequenceGroup<TSequenceGroup> sequenceGroup, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            this.Grouped[sequenceGroup]?.Invoke(param1, param2, param3);
        }

        /// <summary>
        /// Sequence then invoke all delegators.
        /// </summary>
        /// <param name="delegators">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="param1">Parameter to pass when invoking items</param>
        /// <param name="param2">Parameter to pass when invoking items</param>
        /// <param name="param3">Parameter to pass when invoking items</param>
        public static void Invoke(IEnumerable<Delegator<Action<TParam1, TParam2, TParam3>>> delegators, bool sequence, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            foreach (Delegator<Action<TParam1, TParam2, TParam3>> delegator in delegators.OrderBySequenceGroup<Action<TParam1, TParam2, TParam3>, TSequenceGroup>(sequence))
            {
                delegator.Delegate(param1, param2, param3);
            }
        }

        /// <summary>
        /// Sequence then invoke all actions.
        /// </summary>
        /// <param name="actions">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="param1">Parameter to pass when invoking items</param>
        /// <param name="param2">Parameter to pass when invoking items</param>
        /// <param name="param3">Parameter to pass when invoking items</param>
        public static void Invoke(IEnumerable<Action<TParam1, TParam2, TParam3>> actions, bool sequence, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            IEnumerable<Delegator<Action<TParam1, TParam2, TParam3>>> delegators = actions.Select(x => new Delegator<Action<TParam1, TParam2, TParam3>>(x));
            ActionSequenceGroup<TSequenceGroup, TParam1, TParam2, TParam3>.Invoke(delegators, sequence, param1, param2, param3);
        }

        /// <summary>
        /// Sequence then invoke all matching delegates within a collection of instances.
        /// </summary>
        /// <param name="instances">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="param1">Parameter to pass when invoking items</param>
        /// <param name="param2">Parameter to pass when invoking items</param>
        /// <param name="param3">Parameter to pass when invoking items</param>
        public static void Invoke(IEnumerable<object> instances, bool sequence, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            IEnumerable<Delegator<Action<TParam1, TParam2, TParam3>>> delegators = instances.SelectMany(x => x.GetMatchingDelegators<Action<TParam1, TParam2, TParam3>>());
            ActionSequenceGroup<TSequenceGroup, TParam1, TParam2, TParam3>.Invoke(delegators, sequence, param1, param2, param3);
        }
    }

    /// <summary>
    /// Manages a collection of sequenced and grouped actions.
    /// 
    /// These are methods with an attached <see cref="Attributes.SequenceGroupAttribute{TSequenceGroup}"/> attributes.
    /// 
    /// When adding an <see cref="object"/> instance, ALL public methods matching the action signature and having
    /// the required attribute will be added,
    /// </summary>
    /// <typeparam name="TSequenceGroup"></typeparam>
    /// <typeparam name="TParam1"></typeparam>
    /// <typeparam name="TParam2"></typeparam>
    /// <typeparam name="TParam3"></typeparam>
    /// <typeparam name="TParam4"></typeparam>
    /// <typeparam name="TParam4"></typeparam>
    public sealed class ActionSequenceGroup<TSequenceGroup, TParam1, TParam2, TParam3, TParam4> : DelegateSequenceGroup<TSequenceGroup, Action<TParam1, TParam2, TParam3, TParam4>>
        where TSequenceGroup : unmanaged, Enum
    {
        public ActionSequenceGroup(bool sequence) : base(typeof(Action<TParam1, TParam2, TParam3, TParam4>), sequence)
        {
        }
        public ActionSequenceGroup(bool sequence, IEnumerable<Action<TParam1, TParam2, TParam3, TParam4>> actions) : base(typeof(Action<TParam1, TParam2, TParam3, TParam4>), sequence)
        {
            this.Add(actions);
        }
        public ActionSequenceGroup(bool sequence, IEnumerable<object> instances) : base(typeof(Action<TParam1, TParam2, TParam3, TParam4>), sequence)
        {
            this.Add(instances);
        }

        public void Invoke(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            this.Sequenced?.Invoke(param1, param2, param3, param4);
        }

        public void Invoke(SequenceGroup<TSequenceGroup> sequenceGroup, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            this.Grouped[sequenceGroup]?.Invoke(param1, param2, param3, param4);
        }

        /// <summary>
        /// Sequence then invoke all delegators.
        /// </summary>
        /// <param name="delegators">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="param1">Parameter to pass when invoking items</param>
        /// <param name="param2">Parameter to pass when invoking items</param>
        /// <param name="param3">Parameter to pass when invoking items</param>
        /// <param name="param4">Parameter to pass when invoking items</param>
        public static void Invoke(IEnumerable<Delegator<Action<TParam1, TParam2, TParam3, TParam4>>> delegators, bool sequence, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            foreach (Delegator<Action<TParam1, TParam2, TParam3, TParam4>> delegator in delegators.OrderBySequenceGroup<Action<TParam1, TParam2, TParam3, TParam4>, TSequenceGroup>(sequence))
            {
                delegator.Delegate(param1, param2, param3, param4);
            }
        }

        /// <summary>
        /// Sequence then invoke all actions.
        /// </summary>
        /// <param name="actions">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="param1">Parameter to pass when invoking items</param>
        /// <param name="param2">Parameter to pass when invoking items</param>
        /// <param name="param3">Parameter to pass when invoking items</param>
        /// <param name="param4">Parameter to pass when invoking items</param>
        public static void Invoke(IEnumerable<Action<TParam1, TParam2, TParam3, TParam4>> actions, bool sequence, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            IEnumerable<Delegator<Action<TParam1, TParam2, TParam3, TParam4>>> delegators = actions.Select(x => new Delegator<Action<TParam1, TParam2, TParam3, TParam4>>(x));
            ActionSequenceGroup<TSequenceGroup, TParam1, TParam2, TParam3, TParam4>.Invoke(delegators, sequence, param1, param2, param3, param4);
        }

        /// <summary>
        /// Sequence then invoke all matching delegates within a collection of instances.
        /// </summary>
        /// <param name="instances">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="param1">Parameter to pass when invoking items</param>
        /// <param name="param2">Parameter to pass when invoking items</param>
        /// <param name="param3">Parameter to pass when invoking items</param>
        /// <param name="param4">Parameter to pass when invoking items</param>
        public static void Invoke(IEnumerable<object> instances, bool sequence, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            IEnumerable<Delegator<Action<TParam1, TParam2, TParam3, TParam4>>> delegators = instances.SelectMany(x => x.GetMatchingDelegators<Action<TParam1, TParam2, TParam3, TParam4>>());
            ActionSequenceGroup<TSequenceGroup, TParam1, TParam2, TParam3, TParam4>.Invoke(delegators, sequence, param1, param2, param3, param4);
        }
    }

    /// <summary>
    /// Manages a collection of sequenced and grouped actions.
    /// 
    /// These are methods with an attached <see cref="Attributes.SequenceGroupAttribute{TSequenceGroup}"/> attributes.
    /// 
    /// When adding an <see cref="object"/> instance, ALL public methods matching the action signature and having
    /// the required attribute will be added,
    /// </summary>
    /// <typeparam name="TSequenceGroup"></typeparam>
    /// <typeparam name="TParam1"></typeparam>
    /// <typeparam name="TParam2"></typeparam>
    /// <typeparam name="TParam3"></typeparam>
    /// <typeparam name="TParam4"></typeparam>
    /// <typeparam name="TParam5"></typeparam>
    public sealed class ActionSequenceGroup<TSequenceGroup, TParam1, TParam2, TParam3, TParam4, TParam5> : DelegateSequenceGroup<TSequenceGroup, Action<TParam1, TParam2, TParam3, TParam4, TParam5>>
        where TSequenceGroup : unmanaged, Enum
    {
        public ActionSequenceGroup(bool sequence) : base(typeof(Action<TParam1, TParam2, TParam3, TParam4, TParam5>), sequence)
        {
        }
        public ActionSequenceGroup(bool sequence, IEnumerable<Action<TParam1, TParam2, TParam3, TParam4, TParam5>> actions) : base(typeof(Action<TParam1, TParam2, TParam3, TParam4, TParam5>), sequence)
        {
            this.Add(actions);
        }
        public ActionSequenceGroup(bool sequence, IEnumerable<object> instances) : base(typeof(Action<TParam1, TParam2, TParam3, TParam4, TParam5>), sequence)
        {
            this.Add(instances);
        }

        public void Invoke(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            this.Sequenced?.Invoke(param1, param2, param3, param4, param5);
        }

        public void Invoke(SequenceGroup<TSequenceGroup> sequenceGroup, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            this.Grouped[sequenceGroup]?.Invoke(param1, param2, param3, param4, param5);
        }

        /// <summary>
        /// Sequence then invoke all delegators.
        /// </summary>
        /// <param name="delegators">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="param1">Parameter to pass when invoking items</param>
        /// <param name="param2">Parameter to pass when invoking items</param>
        /// <param name="param3">Parameter to pass when invoking items</param>
        /// <param name="param4">Parameter to pass when invoking items</param>
        /// <param name="param5">Parameter to pass when invoking items</param>
        public static void Invoke(IEnumerable<Delegator<Action<TParam1, TParam2, TParam3, TParam4, TParam5>>> delegators, bool sequence, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            foreach (Delegator<Action<TParam1, TParam2, TParam3, TParam4, TParam5>> delegator in delegators.OrderBySequenceGroup<Action<TParam1, TParam2, TParam3, TParam4, TParam5>, TSequenceGroup>(sequence))
            {
                delegator.Delegate(param1, param2, param3, param4, param5);
            }
        }

        /// <summary>
        /// Sequence then invoke all actions.
        /// </summary>
        /// <param name="actions">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="param1">Parameter to pass when invoking items</param>
        /// <param name="param2">Parameter to pass when invoking items</param>
        /// <param name="param3">Parameter to pass when invoking items</param>
        /// <param name="param4">Parameter to pass when invoking items</param>
        /// <param name="param5">Parameter to pass when invoking items</param>
        public static void Invoke(IEnumerable<Action<TParam1, TParam2, TParam3, TParam4, TParam5>> actions, bool sequence, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            IEnumerable<Delegator<Action<TParam1, TParam2, TParam3, TParam4, TParam5>>> delegators = actions.Select(x => new Delegator<Action<TParam1, TParam2, TParam3, TParam4, TParam5>>(x));
            ActionSequenceGroup<TSequenceGroup, TParam1, TParam2, TParam3, TParam4, TParam5>.Invoke(delegators, sequence, param1, param2, param3, param4, param5);
        }

        /// <summary>
        /// Sequence then invoke all matching delegates within a collection of instances.
        /// </summary>
        /// <param name="instances">Items to be invoked</param>
        /// <param name="sequence">When true, items within each sequence group will be ordered via <see cref="Attributes.RequireSequenceGroupAttribute{TSequenceGroup}"/></param>
        /// <param name="param1">Parameter to pass when invoking items</param>
        /// <param name="param2">Parameter to pass when invoking items</param>
        /// <param name="param3">Parameter to pass when invoking items</param>
        /// <param name="param4">Parameter to pass when invoking items</param>
        /// <param name="param5">Parameter to pass when invoking items</param>
        public static void Invoke(IEnumerable<object> instances, bool sequence, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            IEnumerable<Delegator<Action<TParam1, TParam2, TParam3, TParam4, TParam5>>> delegators = instances.SelectMany(x => x.GetMatchingDelegators<Action<TParam1, TParam2, TParam3, TParam4, TParam5>>());
            ActionSequenceGroup<TSequenceGroup, TParam1, TParam2, TParam3, TParam4, TParam5>.Invoke(delegators, sequence, param1, param2, param3, param4, param5);
        }
    }
}