using Guppy.Attributes;
using Guppy.ECS.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Loaders
{
    [Service<IEntityLoader>(ServiceLifetime.Scoped, true)]
    public interface IEntityLoader
    {
        void Configure(IEntityService entities);
    }
}
