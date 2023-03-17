using Guppy.Attributes;
using Guppy.GUI.Providers;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Loaders
{
    [Service<IStyleSheetLoader>(ServiceLifetime.Scoped, true)]
    public interface IStyleSheetLoader
    {
        void Load(IStyleSheetProvider styles);
    }
}
