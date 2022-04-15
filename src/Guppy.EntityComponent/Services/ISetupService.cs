using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Services
{
    public interface ISetupService
    {
        bool TryCreate(IEntity entity);
        bool TryDestroy(IEntity entity);
    }
}
