using Guppy.EntityComponent.DependencyInjection.Builders;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ServiceLoaders
{
    public interface ISerilogLoader : IGuppyLoader
    {
        void RegisterSerilog(LoggerConfiguration loggerConfiguration);
    }
}
