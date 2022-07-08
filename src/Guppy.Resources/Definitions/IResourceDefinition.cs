using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Definitions
{
    public interface IResourceDefinition
    {
        string Name { get; }
        string? Source { get; }
        Type Type { get; }
    }
}
