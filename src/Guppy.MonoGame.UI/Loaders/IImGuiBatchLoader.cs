using Guppy.Attributes;
using Guppy.MonoGame.UI.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Loaders
{
    [Service<IImGuiBatchLoader>(ServiceLifetime.Singleton, true)]
    public interface IImGuiBatchLoader
    {
        void Load(IImGuiBatchProvider batches);
    }
}
