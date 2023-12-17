﻿using Guppy.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Services
{
    internal class ObjectTextFilterService : IObjectTextFilterService
    {
        private ObjectTextFilter[] _filters;
        private Dictionary<Type, ObjectTextFilter> _typeFilters;

        public ObjectTextFilterService(IEnumerable<ObjectTextFilter> filters)
        {
            _filters = filters.OrderBy(x => x.Priority).ToArray();
            _typeFilters = new Dictionary<Type, ObjectTextFilter>();
        }

        public bool Filter(object? instance, string input, int maxDepth = 5, int currentDepth = 0)
        {
            if(instance is null)
            {
                return false;
            }

            if(input.IsNullOrEmpty())
            {
                return true;
            }

            if(currentDepth >= maxDepth)
            {
                return (instance.GetType().AssemblyQualifiedName is string assembly && assembly.Contains(input))
                    || (instance.ToString() is string instanceString && instanceString.Contains(input));
            }

            return this.GetFilter(instance).Filter(instance, input, this, maxDepth, currentDepth);
        }

        private ObjectTextFilter GetFilter(object instance)
        {
            ref ObjectTextFilter? filter = ref CollectionsMarshal.GetValueRefOrAddDefault(_typeFilters, instance.GetType(), out bool exists);
        
            if(exists)
            {
                return filter!;
            }

            filter = _filters.First(x => x.AppliesTo(instance));

            return filter;
        }
    }
}
