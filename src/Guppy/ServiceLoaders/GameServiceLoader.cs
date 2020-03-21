using Guppy.Attributes;
using Guppy.Extensions.Collections;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    public sealed class GameServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            AssemblyHelper.GetTypesAssignableFrom<IGame>().ForEach(t =>
            { // Auto register any IGame class as a scoped type.
                services.AddTypedScoped(t, typeof(IGame));
            });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
