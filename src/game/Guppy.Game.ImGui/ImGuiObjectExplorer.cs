using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Engine.Common.Enums;
using Guppy.Game.ImGui.Services;

namespace Guppy.Game.ImGui
{
    public abstract class ImGuiObjectExplorer
    {
        public virtual int Priority => 0;
        protected internal IImGuiObjectExplorerService explorer { get; internal set; } = null!;

        internal ImGuiObjectExplorer()
        {

        }

        public abstract bool AppliesTo(Type type);
        public abstract TextFilterResult DrawObjectExplorer(int? index, string? name, Type type, object? instance, string filter, int maxDepth, int currentDepth, HashSet<object> tree);
    }

    [Service<ImGuiObjectExplorer>(ServiceLifetime.Singleton, true)]
    public abstract class ImGuiObjectExplorer<T> : ImGuiObjectExplorer
    {
        public override bool AppliesTo(Type type)
        {
            return type.IsAssignableTo(typeof(T));
        }

        public override TextFilterResult DrawObjectExplorer(int? index, string? name, Type type, object? instance, string filter, int maxDepth, int currentDepth, HashSet<object> tree)
        {
            return this.DrawObjectExplorer(
                index: index,
                name: name,
                type: type,
                instance: instance is null ? default : (T?)instance,
                filter: filter,
                maxDepth: maxDepth,
                currentDepth: currentDepth,
                tree: tree);
        }

        protected abstract TextFilterResult DrawObjectExplorer(int? index, string? name, Type type, T? instance, string filter, int maxDepth, int currentDepth, HashSet<object> tree);
    }
}
