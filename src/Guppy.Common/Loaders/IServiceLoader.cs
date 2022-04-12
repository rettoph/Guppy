using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Loaders
{
    public interface IServiceLoader : IGuppyLoader
    {
        void ConfigureServices(IServiceCollection services);
    }
}
