using Guppy.EntityComponent;
using Guppy.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Services
{
    public interface IEntityService : IListService<Guid, IEntity>
    {
        void Initialize();
    }
}
