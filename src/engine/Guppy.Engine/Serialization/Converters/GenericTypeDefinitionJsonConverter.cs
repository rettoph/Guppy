using Guppy.Engine.Common;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Engine.Serialization.Converters
{
    public class GenericTypeDefinitionJsonConverter : JsonConverter<object>
    {
        private readonly Dictionary<Type, GenericTypeJsonConverter> _converters;

        public readonly Type GenericTypeDefinition;
        public readonly Type GenericTypeJsonConverterGenericDefinition;

        public GenericTypeDefinitionJsonConverter(Type genericTypeDefinition, Type genericTypeJsonConverterGenericDefinition)
        {
            ThrowIf.Type.IsNotGenericTypeDefinitionn(genericTypeDefinition);
            ThrowIf.Type.IsNotGenericTypeDefinitionn(genericTypeJsonConverterGenericDefinition);

            _converters = new Dictionary<Type, GenericTypeJsonConverter>();

            this.GenericTypeDefinition = genericTypeDefinition;
            this.GenericTypeJsonConverterGenericDefinition = genericTypeJsonConverterGenericDefinition;
        }

        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.IsGenericType == false)
            {
                return false;
            }

            return typeToConvert.GetGenericTypeDefinition() == this.GenericTypeDefinition;
        }

        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return this.GetJsonConverter(typeToConvert).InternalRead(ref reader, typeToConvert, options);
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            this.GetJsonConverter(value.GetType()).InternalWrite(writer, value, options);
        }

        private GenericTypeJsonConverter GetJsonConverter(Type type)
        {
            ref GenericTypeJsonConverter? converter = ref CollectionsMarshal.GetValueRefOrAddDefault(_converters, type, out bool exists);

            if (exists == false)
            {
                Type genericParamter = type.GenericTypeArguments[0];
                Type converterType = this.GenericTypeJsonConverterGenericDefinition.MakeGenericType(genericParamter);
                converter = (GenericTypeJsonConverter)Activator.CreateInstance(converterType)!;
            }

            return converter!;
        }

        public abstract class GenericTypeJsonConverter
        {
            internal GenericTypeJsonConverter()
            {

            }

            internal abstract object InternalRead(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options);

            internal abstract void InternalWrite(Utf8JsonWriter writer, object value, JsonSerializerOptions options);
        }
        public abstract class GenericTypeJsonConverter<T> : GenericTypeJsonConverter
            where T : struct
        {
            public GenericTypeJsonConverter()
            {

            }

            internal override object InternalRead(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return this.Read(ref reader, typeToConvert, options);
            }

            internal override void InternalWrite(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
            {
                this.Write(writer, (T)value, options);
            }

            protected abstract T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options);

            protected abstract void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options);
        }
    }
}
