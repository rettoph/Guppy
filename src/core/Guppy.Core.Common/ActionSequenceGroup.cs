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
}
