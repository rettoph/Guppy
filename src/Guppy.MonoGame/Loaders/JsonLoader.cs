using Guppy.Loaders;
using Guppy.MonoGame.Json.JsonConverters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Loaders
{
    internal sealed class JsonLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<JsonConverter, ColorJsonConverter>();

            services.AddTransient<JsonSerializerOptions>(p =>
            {
                var options = new JsonSerializerOptions();

                foreach(JsonConverter converter in p.GetServices<JsonConverter>())
                {
                    options.Converters.Add(converter);
                }

                return options;
            });
        }
    }
}
