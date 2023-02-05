using Guppy.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Attributes
{
    internal sealed class ServiceLoaderAutoLoadAttribute : AutoLoadingAttribute
    {
        protected override void Initialize(GuppyEngine engine, Type classType)
        {
            var loader = Activator.CreateInstance(classType) as IServiceLoader;

            engine.AddServiceLoader(loader ?? throw new NotImplementedException());
        }
    }
}
