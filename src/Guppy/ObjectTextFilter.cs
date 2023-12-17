using Guppy.Attributes;
using Guppy.Common.Services;
using Guppy.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
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

        public abstract bool Filter(object instance, string input, IObjectTextFilterService filter, int maxDepth, int currentDepth);
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

        public override bool Filter(object instance, string input, IObjectTextFilterService filter, int maxDepth, int currentDepth)
        {
            return this.Filter((T)instance, input, filter, currentDepth, maxDepth);
        }

        protected abstract bool Filter(T instance, string input, IObjectTextFilterService filter, int maxDepth, int currentDepth);
    }
}
