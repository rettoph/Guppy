using Guppy.Attributes;
using Guppy.Common.Enums;
using Guppy.Common.Services;
using Guppy.Enums;
using Guppy.Game.ImGui.Services;
using Microsoft.Xna.Framework;
using System.Text;

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
        public abstract TextFilterResult DrawObjectExplorer(int? index, string? name, Type type, object? instance, string filter, int maxDepth, int currentDepth);
    }

    [Service<ImGuiObjectExplorer>(ServiceLifetime.Singleton, true)]
    public abstract class ImGuiObjectExplorer<T> : ImGuiObjectExplorer
    {
        public override bool AppliesTo(Type type)
        {
            return type.IsAssignableTo(typeof(T));
        }

        public override TextFilterResult DrawObjectExplorer(int? index, string? name, Type type, object? instance, string filter, int maxDepth, int currentDepth)
        {
            return this.DrawObjectExplorer(
                index: index, 
                name: name, 
                type: type, 
                instance: instance is null ? default : (T?)instance, 
                filter: filter,
                maxDepth: maxDepth, 
                currentDepth: currentDepth);
        }

        protected abstract TextFilterResult DrawObjectExplorer(int? index, string? name, Type type, T? instance, string filter, int maxDepth, int currentDepth, IImGuiObjectExplorerService explorer);
    }
}
