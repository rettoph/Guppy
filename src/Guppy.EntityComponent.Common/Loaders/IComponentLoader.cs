using Guppy.Loaders;
using Guppy.EntityComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Loaders
{
    public interface IComponentLoader : IGuppyLoader
    {
        void ConfigureComponents(IComponentCollection components);
    }
}
