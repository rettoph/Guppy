using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.DependencyInjection.Interfaces
{
    public interface IServiceFilter
    {
        void Initialize(IServiceProvider provier);

        bool AppliesTo(IServiceConfiguration service);

        bool Invoke(IServiceProvider provider, IServiceConfiguration service, object? configuration);
    }
}
