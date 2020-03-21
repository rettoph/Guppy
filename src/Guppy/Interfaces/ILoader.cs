using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface ILoader : IService
    {
        void Load(ServiceProvider provider);
    }
}
