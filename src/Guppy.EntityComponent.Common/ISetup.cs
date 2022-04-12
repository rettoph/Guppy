using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent
{
    public interface ISetup
    {
        bool TryCreate(IEntity entity);
        bool TryDestroy(IEntity entity);
    }
}
