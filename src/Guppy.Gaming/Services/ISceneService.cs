using Guppy.EntityComponent.Services;
using Guppy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Services
{
    public interface ISceneService : IListService<Guid, Scene>
    {
        Scene? Scene { get; set; }

        event OnChangedEventDelegate<ISceneService, Scene?>? OnSceneChanged;
    }
}
