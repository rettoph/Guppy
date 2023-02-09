using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Configurations;

namespace Guppy
{
    public interface IGuppyConfigurator
    {
        void Configure(GuppyConfiguration configuration);
    }
}
