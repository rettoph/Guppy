using Guppy.Definitions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    internal sealed class SettingSerializerProvider : ISettingSerializerProvider
    {
        private Dictionary<Type, SettingSerializer> _serializers;

        public SettingSerializerProvider(IEnumerable<SettingSerializerDefinition> definitions)
        {
            _serializers = new Dictionary<Type, SettingSerializer>(definitions.Count());

            foreach(SettingSerializerDefinition definition in definitions)
            {
                var serializer = definition.BuildSerializer();
                _serializers.Add(serializer.Type, serializer);
            }
        }

        public bool TryGet<T>([MaybeNullWhen(false)] out SettingSerializer<T> serializer)
        {
            if(_serializers.TryGetValue(typeof(T), out SettingSerializer? downcast) && downcast is SettingSerializer<T> casted)
            {
                serializer = casted;
                return true;
            }

            serializer = null;
            return false;
        }
    }
}
