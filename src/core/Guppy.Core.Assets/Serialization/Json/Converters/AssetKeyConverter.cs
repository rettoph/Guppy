using System.Text.Json;
using System.Text.Json.Serialization;
using Guppy.Core.Assets.Common;
using Guppy.Core.Assets.Common.Services;

namespace Guppy.Core.Assets.Serialization.Json.Converters
{
    internal class AssetKeyConverter(IAssetService assetService) : JsonConverter<object>
    {
        private readonly IAssetService _assetService = assetService;

        private interface IAssetGetter
        {
            object GetAsset(string key, IAssetService assetService);
        }
        private class AssetValueGetter<T> : IAssetGetter
            where T : notnull
        {
            public object GetAsset(string key, IAssetService assetService)
            {
                return AssetKey<T>.Get(key);
            }
        }

        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.IsGenericType == false)
            {
                return false;
            }

            bool result = typeToConvert.GetGenericTypeDefinition() == typeof(AssetKey<>);
            return result;
        }

        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            reader.CheckToken(JsonTokenType.String, true);
            string key = reader.ReadString();

            Type getterType = typeof(AssetValueGetter<>).MakeGenericType(typeToConvert.GenericTypeArguments[0]);
            IAssetGetter? getter = (IAssetGetter)(Activator.CreateInstance(getterType) ?? throw new NotImplementedException());
            object instance = getter.GetAsset(key, this._assetService);

            return instance;
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}