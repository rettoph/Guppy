using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent
{
    public interface IInitializable : IEntity
    {
        void Initialize();
        void Uninitialize();
    }
}
