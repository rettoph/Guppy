using Guppy.Engine.Common.Enums;

namespace Guppy.Game.ImGui.Services
{
    public interface IImGuiObjectExplorerService
    {
        TextFilterResult DrawObjectExplorer(object instance, string filter = "", int maxDepth = 5, HashSet<object>? tree = null);
        TextFilterResult DrawObjectExplorer(int? index, string? name, Type type, object? instance, string filter, int maxDepth, int currentDepth, HashSet<object> tree);
    }
}
