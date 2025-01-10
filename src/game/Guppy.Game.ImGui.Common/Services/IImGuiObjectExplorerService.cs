using Guppy.Engine.Common.Enums;

namespace Guppy.Game.ImGui.Common.Services
{
    public interface IImGuiObjectExplorerService
    {
        TextFilterResultEnum DrawObjectExplorer(object instance, string filter = "", int maxDepth = 5, HashSet<object>? tree = null);
        TextFilterResultEnum DrawObjectExplorer(int? index, string? name, Type type, object? instance, string filter, int maxDepth, int currentDepth, HashSet<object> tree);
    }
}