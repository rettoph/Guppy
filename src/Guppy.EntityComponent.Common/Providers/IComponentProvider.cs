using Guppy.EntityComponent.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Providers
{
    public interface IComponentProvider
    {
        IComponentService Create(Type entity, IServiceProvider provider);
    }
}
