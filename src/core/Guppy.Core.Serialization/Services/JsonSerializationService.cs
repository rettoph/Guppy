using Guppy.Core.Common;
using Guppy.Core.Serialization.Common.Services;
using Serilog;
using System.Text.Json;
using STJ = System.Text.Json;

namespace Guppy.Core.Serialization.Services
{
    internal sealed class JsonSerializationService(ILogger logger, IDefaultInstanceService defaultInstances, IConfiguration<JsonSerializerOptions> options) : IJsonSerializationService
    {
        private readonly JsonSerializerOptions _options = options.Value;
        private readonly ILogger _logger = logger;
        private readonly IDefaultInstanceService _defaultInstances = defaultInstances;

        public T Deserialize<T>(string json, out bool success)
        {
            if (json == string.Empty)
            {
                success = false;
                return _defaultInstances.Get<T>();
            }

            try
            {
                success = true;
                return STJ.JsonSerializer.Deserialize<T>(json, _options) ?? _defaultInstances.Get<T>();
            }
            catch (Exception e)
            {
                success = false;
                _logger.Error(e, "{ClassName}::{MethodName} - Exception deserializaing Json<{Type}> => '{JSON}'", nameof(JsonSerializationService), nameof(Deserialize), typeof(T).Name, json);
                return _defaultInstances.Get<T>();
            }
        }
        public T Deserialize<T>(Stream utf8Json, out bool success)
        {
            try
            {
                success = true;
                return STJ.JsonSerializer.Deserialize<T>(utf8Json, _options) ?? _defaultInstances.Get<T>();
            }
            catch (Exception e)
            {
                success = false;
                _logger.Error(e, "{ClassName}::{MethodName} - Exception deserializaing Json<{Type}>", nameof(JsonSerializationService), nameof(Deserialize), typeof(T).Name);
                return _defaultInstances.Get<T>();
            }
        }
        public T Deserialize<T>(ref Utf8JsonReader reader, out bool success)
        {
            try
            {
                success = true;
                return STJ.JsonSerializer.Deserialize<T>(ref reader, _options) ?? _defaultInstances.Get<T>();
            }
            catch (Exception e)
            {
                success = false;
                _logger.Error(e, "{ClassName}::{MethodName} - Exception deserializaing Json<{Type}>", nameof(JsonSerializationService), nameof(Deserialize), typeof(T).Name);
                return _defaultInstances.Get<T>();
            }
        }

        public T Deserialize<T>(ref JsonElement json, out bool success)
        {
            try
            {
                success = true;
                return STJ.JsonSerializer.Deserialize<T>(json, _options) ?? _defaultInstances.Get<T>();
            }
            catch (Exception e)
            {
                success = false;
                _logger.Error(e, "{ClassName}::{MethodName} - Exception deserializaing Json<{Type}> => '{JSON}'", nameof(JsonSerializationService), nameof(Deserialize), typeof(T).Name, json);
                return _defaultInstances.Get<T>();
            }
        }

        public string Serialize<T>(T obj)
        {
            return STJ.JsonSerializer.Serialize(obj, _options);
        }
        public void Serialize<T>(Stream utf8Json, T obj)
        {
            STJ.JsonSerializer.Serialize(utf8Json, obj, _options);
        }
        public void Serialize<T>(Utf8JsonWriter writer, T obj)
        {
            STJ.JsonSerializer.Serialize(writer, obj, _options);
        }
    }
}
