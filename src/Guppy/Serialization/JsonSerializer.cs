﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Guppy.Serialization;
using Serilog;
using STJ = System.Text.Json;

namespace Guppy.Serialization
{
    internal sealed class JsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions _options;
        private readonly ILogger _logger;

        public JsonSerializer(ILogger logger, IEnumerable<JsonConverter> converters)
        {
            _logger = logger;
            _options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            foreach (JsonConverter converter in converters)
            {
                _options.Converters.Add(converter);
            }
        }

        public T? Deserialize<T>(string json, out bool success)
        {
            if (json == string.Empty)
            {
                success = false;
                return default!;
            }

            try
            {
                success = true;
                return STJ.JsonSerializer.Deserialize<T>(json, _options);
            }
            catch (Exception e)
            {
                success = false;
                _logger.Error(e, "{ClassName}::{MethodName} - Exception deserializaing Json<{Type}> => '{JSON}'", nameof(JsonSerializer), nameof(Deserialize), typeof(T).Name, json);
                return default!;
            }
        }
        public T? Deserialize<T>(Stream utf8Json, out bool success)
        {
            try
            {
                success = true;
                return STJ.JsonSerializer.Deserialize<T>(utf8Json, _options);
            }
            catch (Exception e)
            {
                success = false;
                _logger.Error(e, "{ClassName}::{MethodName} - Exception deserializaing Json<{Type}>", nameof(JsonSerializer), nameof(Deserialize), typeof(T).Name);
                return default!;
            }
        }
        public T? Deserialize<T>(ref Utf8JsonReader reader, out bool success)
        {
            try
            {
                success = true;
                return STJ.JsonSerializer.Deserialize<T>(ref reader, _options);
            }
            catch (Exception e)
            {
                success = false;
                _logger.Error(e, "{ClassName}::{MethodName} - Exception deserializaing Json<{Type}>", nameof(JsonSerializer), nameof(Deserialize), typeof(T).Name);
                return default!;
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
    }
}