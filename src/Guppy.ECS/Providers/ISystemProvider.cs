using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Providers
{
    public interface ISystemProvider
    {
        IEnumerable<ISystem> Create(IServiceProvider provider);
    }
}
