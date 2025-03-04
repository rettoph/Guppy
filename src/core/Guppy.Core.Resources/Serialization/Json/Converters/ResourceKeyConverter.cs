﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;

namespace Guppy.Core.Resources.Serialization.Json.Converters
{
    internal class ResourceKeyConverter(IResourceService resourceService) : JsonConverter<object>
    {
        private readonly IResourceService _resourceService = resourceService;

        private interface IResourceGetter
        {
            object GetResource(string key, IResourceService resourceService);
        }
        private class ResourceValueGetter<T> : IResourceGetter
            where T : notnull
        {
            public object GetResource(string key, IResourceService resourceService)
            {
                return ResourceKey<T>.Get(key);
            }
        }

        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.IsGenericType == false)
            {
                return false;
            }

            bool result = typeToConvert.GetGenericTypeDefinition() == typeof(ResourceKey<>);
            return result;
        }

        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            reader.CheckToken(JsonTokenType.String, true);
            string key = reader.ReadString();

            Type getterType = typeof(ResourceValueGetter<>).MakeGenericType(typeToConvert.GenericTypeArguments[0]);
            IResourceGetter? getter = (IResourceGetter)(Activator.CreateInstance(getterType) ?? throw new NotImplementedException());
            object instance = getter.GetResource(key, this._resourceService);

            return instance;
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}