using Guppy.Engine.Attributes;
using Guppy.Engine.Common.Enums;
using Guppy.Engine.Common.Services;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Guppy.Engine
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

        public override TextFilterResult Filter(object instance, string input, IObjectTextFilterService filter, int maxDepth, int currentDepth, HashSet<object> tree)
        {
            Type type = instance.GetType();

            if (instance.ToString() is string instanceString && instanceString.Contains(input))
            {
                return TextFilterResult.Matched;
            }

            if (type.AssemblyQualifiedName is string assembly && assembly.Contains(input))
            {
                return TextFilterResult.Matched;
            }

            var (fields, properties) = this.GetTypeInfo(type);
            if (properties.Length == 0 && fields.Length == 0)
            {
                return TextFilterResult.NotMatched;
            }

            foreach (PropertyInfo property in properties)
            {
                object? propertyValue = property.GetValue(instance);

                if (filter.Filter(propertyValue, input, maxDepth, currentDepth + 1, tree) == TextFilterResult.Matched)
                {
                    return TextFilterResult.Matched;
                }
            }

            foreach (FieldInfo field in fields)
            {
                object? fieldValue = field.GetValue(instance);

                if (filter.Filter(fieldValue, input, maxDepth, currentDepth + 1, tree) == TextFilterResult.Matched)
                {
                    return TextFilterResult.Matched;
                }
            }

            if (instance is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    if (filter.Filter(item, input, maxDepth, currentDepth + 1, tree) == TextFilterResult.Matched)
                    {
                        return TextFilterResult.Matched;
                    }
                }
            }

            return TextFilterResult.NotMatched;
        }

        private (FieldInfo[], PropertyInfo[]) GetTypeInfo(Type type)
        {
            ref (FieldInfo[], PropertyInfo[]) info = ref CollectionsMarshal.GetValueRefOrAddDefault(_typeInfo, type, out bool exists);

            if (exists)
            {
                return info;
            }

            info.Item1 = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => typeof(Delegate).IsAssignableFrom(x.FieldType) == false)
                .ToArray();

            info.Item2 = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                .Where(x => x.GetMethod!.GetParameters().Length == 0)
                .Where(x => typeof(Delegate).IsAssignableFrom(x.PropertyType) == false)
                .ToArray();

            return info;
        }
    }
}
