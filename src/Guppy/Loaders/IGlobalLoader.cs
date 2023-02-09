using Guppy.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Loaders
{
    [Service<IGlobalLoader>(ServiceLifetime.Singleton, true)]
    public interface IGlobalLoader
    {
        void Load(GuppyEngine engine);
    }
}
