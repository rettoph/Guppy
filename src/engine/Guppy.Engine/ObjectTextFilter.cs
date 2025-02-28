﻿using Guppy.Engine.Common.Enums;
using Guppy.Engine.Common.Services;

namespace Guppy.Engine
{
    public abstract class ObjectTextFilter
    {
        public readonly int Priority;

        internal ObjectTextFilter(int priority)
        {
            this.Priority = priority;
        }

        public abstract bool AppliesTo(object instance);

        public abstract TextFilterResultEnum Filter(object instance, string input, IObjectTextFilterService filter, int maxDepth, int currentDepth, HashSet<object> tree);
    }

    public abstract class ObjectTextFilter<T>(int priority = 0) : ObjectTextFilter(priority)
    {
        public override bool AppliesTo(object instance)
        {
            return instance is T;
        }

        public override TextFilterResultEnum Filter(object instance, string input, IObjectTextFilterService filter, int maxDepth, int currentDepth, HashSet<object> tree)
        {
            return this.Filter((T)instance, input, filter, currentDepth, maxDepth, tree);
        }

        protected abstract TextFilterResultEnum Filter(T instance, string input, IObjectTextFilterService filter, int maxDepth, int currentDepth, HashSet<object> tree);
    }
}