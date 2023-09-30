﻿using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.Serialization
{
    internal class DefaultJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions _options;
        private ILogger _logger;

        public DefaultJsonSerializer(JsonSerializerOptions options, ILogger logger)
        {
            _options = options;
            _logger = logger;
        }

        public T? Deserialize<T>(string json, out bool success)
        {
            if(json == string.Empty)
            {
                success = false;
                return default!;
            }

            try
            {
                success = true;
                return JsonSerializer.Deserialize<T>(json, _options);
            }
            catch(Exception e)
            {
                success = false;
                _logger.Error(e, "{ClassName}::{MethodName} - Exception deserializaing Json<{Type}> => '{JSON}'", nameof(DefaultJsonSerializer), nameof(Deserialize), typeof(T).Name, json);
                return default!;
            }
        }

        public string Serialize<T>(T value)
        {
            return JsonSerializer.Serialize<T>(value, _options);
        }
    }
}