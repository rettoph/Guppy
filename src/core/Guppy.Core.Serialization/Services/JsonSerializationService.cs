using System.Text.Json;
using Guppy.Core.Common;
using Guppy.Core.Serialization.Common.Services;
using STJ = System.Text.Json;

namespace Guppy.Core.Serialization.Services
{
    internal sealed class JsonSerializationService(IDefaultInstanceService defaultInstances, IConfiguration<JsonSerializerOptions> options) : IJsonSerializationService
    {
        private readonly JsonSerializerOptions _options = options.Value;
        private readonly IDefaultInstanceService _defaultInstanceService = defaultInstances;

        public T Deserialize<T>(string json, out bool success)
        {
            if (json == string.Empty)
            {
                success = false;
                return this._defaultInstanceService.Get<T>();
            }

            try
            {
                success = true;
                return STJ.JsonSerializer.Deserialize<T>(json, this._options) ?? this._defaultInstanceService.Get<T>();
            }
            catch (Exception e)
            {
                success = false;

#if DEBUG
                throw;
#else
                return _defaultInstanceService.Get<T>();
#endif
            }
        }
        public T Deserialize<T>(Stream utf8Json, out bool success)
        {
            try
            {
                success = true;
                return STJ.JsonSerializer.Deserialize<T>(utf8Json, this._options) ?? this._defaultInstanceService.Get<T>();
            }
            catch (Exception e)
            {
                success = false;
#if DEBUG
                throw;
#else
                return _defaultInstanceService.Get<T>();
#endif
            }
        }
        public T Deserialize<T>(ref Utf8JsonReader reader, out bool success)
        {
            try
            {
                success = true;
                return STJ.JsonSerializer.Deserialize<T>(ref reader, this._options) ?? this._defaultInstanceService.Get<T>();
            }
            catch (Exception e)
            {
                success = false;
#if DEBUG
                throw;
#else
                return _defaultInstanceService.Get<T>();
#endif
            }
        }

        public T Deserialize<T>(ref JsonElement json, out bool success)
        {
            try
            {
                success = true;
                return STJ.JsonSerializer.Deserialize<T>(json, this._options) ?? this._defaultInstanceService.Get<T>();
            }
            catch (Exception e)
            {
                success = false;
#if DEBUG
                throw;
#else
                return _defaultInstanceService.Get<T>();
#endif
            }
        }

        public string Serialize<T>(T obj)
        {
            return STJ.JsonSerializer.Serialize(obj, this._options);
        }

        public void Serialize<T>(Stream utf8Json, T obj)
        {
            STJ.JsonSerializer.Serialize(utf8Json, obj, this._options);
        }

        public void Serialize<T>(Utf8JsonWriter writer, T obj)
        {
            STJ.JsonSerializer.Serialize(writer, obj, this._options);
        }
    }
}