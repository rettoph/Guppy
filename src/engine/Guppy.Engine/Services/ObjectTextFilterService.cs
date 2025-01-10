using System.Runtime.InteropServices;
using Guppy.Core.Common.Extensions.System;
using Guppy.Engine.Common.Enums;
using Guppy.Engine.Common.Services;

namespace Guppy.Engine.Services
{
    internal class ObjectTextFilterService(IEnumerable<ObjectTextFilter> filters) : IObjectTextFilterService
    {
        private readonly ObjectTextFilter[] _filters = [.. filters.OrderBy(x => x.Priority)];
        private readonly Dictionary<Type, ObjectTextFilter> _typeFilters = [];

        public TextFilterResultEnum Filter(object? instance, string input, int maxDepth = 5, int currentDepth = 0, HashSet<object>? tree = null)
        {
            tree ??= [];

            if (instance is null)
            {
                return TextFilterResultEnum.NotMatched;
            }

            if (input.IsNullOrEmpty())
            {
                return TextFilterResultEnum.None;
            }

            if (currentDepth >= maxDepth || (instance.GetType().IsValueType == false && tree.Add(instance) == false && currentDepth > 0))
            {
                return (instance.GetType().AssemblyQualifiedName is string assembly && assembly.Contains(input))
                    || (instance.ToString() is string instanceString && instanceString.Contains(input))
                    ? TextFilterResultEnum.Matched : TextFilterResultEnum.NotMatched;
            }

            return this.GetFilter(instance).Filter(instance, input, this, maxDepth, currentDepth, tree);
        }

        private ObjectTextFilter GetFilter(object instance)
        {
            ref ObjectTextFilter? filter = ref CollectionsMarshal.GetValueRefOrAddDefault(this._typeFilters, instance.GetType(), out bool exists);

            if (exists)
            {
                return filter!;
            }

            filter = this._filters.First(x => x.AppliesTo(instance));

            return filter;
        }
    }
}