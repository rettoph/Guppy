using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Definitions
{
    public interface ISystemDefinition
    {
        int Order { get; }
        Type Type { get; }
        Type[] Filters { get; }
    }
}
