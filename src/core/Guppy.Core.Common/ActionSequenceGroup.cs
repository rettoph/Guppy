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
            this.Sequenced?.Invoke(param);
        }

        public void Invoke(SequenceGroup<TSequenceGroup> sequenceGroup, TParam param)
        {
            this.Grouped[sequenceGroup]?.Invoke(param);
        }

        public static void Invoke(IEnumerable<Delegator<Action<TParam>>> delegators, TParam param)
        {
            foreach (Delegator<Action<TParam>> delegator in delegators.Sequence<Action<TParam>, TSequenceGroup>())
            {
                delegator.Delegate(param);
            }
        }

        public static void Invoke(IEnumerable<Action<TParam>> actions, TParam param)
        {
            IEnumerable<Delegator<Action<TParam>>> delegators = actions.Select(x => new Delegator<Action<TParam>>(x));
            ActionSequenceGroup<TSequenceGroup, TParam>.Invoke(delegators, param);
        }

        public static void Invoke(IEnumerable<object> instances, TParam param)
        {
            IEnumerable<Delegator<Action<TParam>>> delegators = instances.SelectMany(x => x.GetMatchingDelegators<Action<TParam>>());
            ActionSequenceGroup<TSequenceGroup, TParam>.Invoke(delegators, param);
        }
    }
}
