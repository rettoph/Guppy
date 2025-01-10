using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using Guppy.Engine.Common.Enums;
using Guppy.Engine.Common.Services;

namespace Guppy.Engine
{

    internal class DefaultObjectTextFilter : ObjectTextFilter
    {
        private readonly Dictionary<Type, (FieldInfo[], PropertyInfo[])> _typeInfo;

        public DefaultObjectTextFilter() : base(int.MaxValue)
        {
            this._typeInfo = [];
        }

        public override bool AppliesTo(object instance)
        {
            return true;
        }

        public override TextFilterResultEnum Filter(object instance, string input, IObjectTextFilterService filter, int maxDepth, int currentDepth, HashSet<object> tree)
        {
            Type type = instance.GetType();

            if (instance.ToString() is string instanceString && instanceString.Contains(input))
            {
                return TextFilterResultEnum.Matched;
            }

            if (type.AssemblyQualifiedName is string assembly && assembly.Contains(input))
            {
                return TextFilterResultEnum.Matched;
            }

            var (fields, properties) = this.GetTypeInfo(type);
            if (properties.Length == 0 && fields.Length == 0)
            {
                return TextFilterResultEnum.NotMatched;
            }

            foreach (PropertyInfo property in properties)
            {
                object? propertyValue = property.GetValue(instance);

                if (filter.Filter(propertyValue, input, maxDepth, currentDepth + 1, tree) == TextFilterResultEnum.Matched)
                {
                    return TextFilterResultEnum.Matched;
                }
            }

            foreach (FieldInfo field in fields)
            {
                object? fieldValue = field.GetValue(instance);

                if (filter.Filter(fieldValue, input, maxDepth, currentDepth + 1, tree) == TextFilterResultEnum.Matched)
                {
                    return TextFilterResultEnum.Matched;
                }
            }

            if (instance is IEnumerable enumerable)
            {
                foreach (object? item in enumerable)
                {
                    if (filter.Filter(item, input, maxDepth, currentDepth + 1, tree) == TextFilterResultEnum.Matched)
                    {
                        return TextFilterResultEnum.Matched;
                    }
                }
            }

            return TextFilterResultEnum.NotMatched;
        }

        private (FieldInfo[], PropertyInfo[]) GetTypeInfo(Type type)
        {
            ref (FieldInfo[], PropertyInfo[]) info = ref CollectionsMarshal.GetValueRefOrAddDefault(this._typeInfo, type, out bool exists);

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