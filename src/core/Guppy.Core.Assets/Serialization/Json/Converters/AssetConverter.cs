using System.Text.Json;
using System.Text.Json.Serialization;
using Guppy.Core.Assets.Common;
using Guppy.Core.Assets.Common.Services;

namespace Guppy.Core.Assets.Serialization.Json.Converters
{
    internal class AssetConverter(IAssetService assetService) : JsonConverter<object>
    {
        private readonly IAssetService _assetService = assetService;

        private interface IAssetValueGetter
        {
            object GetAssetValue(string key, IAssetService assetService);
        }
        private class AssetValueGetter<T> : IAssetValueGetter
            where T : notnull
        {
            public object GetAssetValue(string key, IAssetService assetService)
            {
                return assetService.Get<T>(AssetKey<T>.Get(key));
            }
        }

        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.IsGenericType == false)
            {
                return false;
            }

            bool result = typeToConvert.GetGenericTypeDefinition() == typeof(Asset<>);
            return result;
        }

        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            reader.CheckToken(JsonTokenType.String, true);
            string key = reader.ReadString();

            Type getterType = typeof(AssetValueGetter<>).MakeGenericType(typeToConvert.GenericTypeArguments[0]);
            IAssetValueGetter? getter = (IAssetValueGetter)(Activator.CreateInstance(getterType) ?? throw new NotImplementedException());
            object instance = getter.GetAssetValue(key, this._assetService);

            return instance;
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}