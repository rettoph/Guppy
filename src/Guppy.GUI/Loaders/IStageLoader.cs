using Guppy.Attributes;
using Guppy.Common.Utilities;
using Guppy.GUI.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Loaders
{
    [Service<IStageLoader>(ServiceLifetime.Scoped, true)]
    public interface IStageLoader
    {
        BlockList StageBlockList { get; }

        void Load(Stage stage);
    }
}
