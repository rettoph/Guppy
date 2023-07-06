using Guppy.Attributes;
using Guppy.Common.Utilities;
using Guppy.Enums;

namespace Guppy.GUI.Loaders
{
    [Service<IStageLoader>(ServiceLifetime.Scoped, true)]
    public interface IStageLoader
    {
        BlockList StageBlockList { get; }

        void Load(Stage stage);
    }
}
