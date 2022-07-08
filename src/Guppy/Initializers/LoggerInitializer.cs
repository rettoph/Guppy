using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Initializers
{
    [AutoLoad]
    internal sealed class LoggerInitializer : GuppyInitializer<ILoggerLoader>
    {
        protected override void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<ILoggerLoader> loaders)
        {
            var conf = new LoggerConfiguration();

            foreach(ILoggerLoader loader in loaders)
            {
                loader.ConfigureLogging(conf);
            }

            var log = conf.CreateLogger();

            services.AddSingleton<ILogger>(log);
            services.AddSingleton<Logger>(log);
        }
    }
}
