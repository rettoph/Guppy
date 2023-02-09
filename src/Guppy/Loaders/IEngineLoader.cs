using Guppy.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Loaders
{
    [Service<IEngineLoader>(ServiceLifetime.Singleton, true)]
    public interface IEngineLoader
    {
        void Load(GuppyEngine engine);
    }
}
