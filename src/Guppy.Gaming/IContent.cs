using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming
{
    public interface IContent : IResource
    {
        public string Path { get; }
    }
}
