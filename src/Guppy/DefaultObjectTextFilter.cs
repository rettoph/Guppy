using Guppy.Attributes;
using Guppy.Common.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{

    [AutoLoad]
    internal class DefaultObjectTextFilter : ObjectTextFilter
    {
        private Dictionary<Type, (FieldInfo[], PropertyInfo[])> _typeInfo;

        public DefaultObjectTextFilter() : base(int.MaxValue)
        {
            _typeInfo = new Dictionary<Type, (FieldInfo[], PropertyInfo[])>();
        }

        public override bool AppliesTo(object instance)
        {
            return true;
        }

        public override bool Filter(object instance, string input, IObjectTextFilterService filter, int maxDepth, int currentDepth)
        {
            Type type = instance.GetType();

            if (type.IsPrimitive || type == typeof(string))
            {
                return (instance.ToString() ?? string.Empty).Contains(input);
            }

            if(type.AssemblyQualifiedName is string assembly && assembly.Contains(input))
            {
                return true;
            }

            var (fields, properties) = this.GetTypeInfo(type);
            if (properties.Length == 0 && fields.Length == 0)
            {
                return false;
            }

            foreach (PropertyInfo property in properties)
            {
                object? propertyValue = property.GetValue(instance);

                if(filter.Filter(propertyValue, input, maxDepth, currentDepth + 1))
                {
                    return true;
                }
            }

            foreach (FieldInfo field in fields)
            {
                object? fieldValue = field.GetValue(instance);

                if (filter.Filter(fieldValue, input, maxDepth, currentDepth + 1))
                {
                    return true;
                }
            }

            if (instance is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    if (filter.Filter(item, input, maxDepth, currentDepth + 1))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private (FieldInfo[], PropertyInfo[]) GetTypeInfo(Type type)
        {
            ref (FieldInfo[], PropertyInfo[]) info = ref CollectionsMarshal.GetValueRefOrAddDefault(_typeInfo, type, out bool exists);

            if(exists)
            {
                return info;
            }

            info.Item1 = type.GetFields(BindingFlags.Public | BindingFlags.Instance).ToArray();
            info.Item2 = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                .Where(x => x.GetMethod!.GetParameters().Length == 0)
                .ToArray();

            return info;
        }
    }
}
