using Guppy.EntityComponent.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent
{
    public interface IEntity
    {
        Guid Id { get; }
        IComponentService Components { get; internal set; }
    }
}
