using Guppy.Engine.Common.Enums;
using Guppy.Game.ImGui.Common.Services;

namespace Guppy.Game.ImGui.Common
{
    public abstract class ImGuiObjectExplorer
    {
        public virtual int Priority => 0;
        protected internal IImGuiObjectExplorerService explorer { get; internal set; } = null!;

        internal ImGuiObjectExplorer()
        {

        }

        public abstract bool AppliesTo(Type type);
        public abstract TextFilterResultEnum DrawObjectExplorer(int? index, string? name, Type type, object? instance, string filter, int maxDepth, int currentDepth, HashSet<object> tree);
    }

    public abstract class ImGuiObjectExplorer<T> : ImGuiObjectExplorer
    {
        public override bool AppliesTo(Type type) => type.IsAssignableTo(typeof(T));

        public override TextFilterResultEnum DrawObjectExplorer(int? index, string? name, Type type, object? instance, string filter, int maxDepth, int currentDepth, HashSet<object> tree) => this.DrawObjectExplorer(
                index: index,
                name: name,
                type: type,
                instance: instance is null ? default : (T?)instance,
                filter: filter,
                maxDepth: maxDepth,
                currentDepth: currentDepth,
                tree: tree);

        protected abstract TextFilterResultEnum DrawObjectExplorer(int? index, string? name, Type type, T? instance, string filter, int maxDepth, int currentDepth, HashSet<object> tree);
    }
}