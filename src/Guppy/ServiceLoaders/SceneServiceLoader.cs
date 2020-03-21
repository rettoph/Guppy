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
    internal sealed class SceneServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            AssemblyHelper.GetTypesAssignableFrom<IScene>().ForEach(t =>
            { // Auto register any IGame class as a scoped type.
                services.AddTypedScoped(t, typeof(IScene));
            });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
