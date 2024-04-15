using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Engine.Common.Enums;
using Guppy.Engine.Common.Services;

namespace Guppy.Engine
{
    [Service<ObjectTextFilter>(ServiceLifetime.Singleton, true)]
    public abstract class ObjectTextFilter
    {
        public readonly int Priority;

        internal ObjectTextFilter(int priority)
        {
            this.Priority = priority;
        }

        public abstract bool AppliesTo(object instance);

        public abstract TextFilterResult Filter(object instance, string input, IObjectTextFilterService filter, int maxDepth, int currentDepth, HashSet<object> tree);
    }

    public abstract class ObjectTextFilter<T> : ObjectTextFilter
    {
        public ObjectTextFilter(int priority = 0) : base(priority)
        {
        }

        public override bool AppliesTo(object instance)
        {
            return instance is T;
        }

        public override TextFilterResult Filter(object instance, string input, IObjectTextFilterService filter, int maxDepth, int currentDepth, HashSet<object> tree)
        {
            return this.Filter((T)instance, input, filter, currentDepth, maxDepth, tree);
        }

        protected abstract TextFilterResult Filter(T instance, string input, IObjectTextFilterService filter, int maxDepth, int currentDepth, HashSet<object> tree);
    }
}
