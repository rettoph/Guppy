using System.Runtime.InteropServices;
using Guppy.Engine.Common.Enums;

namespace Guppy.Game.ImGui.Common.Services
{
    internal class ImGuiObjectExplorerService : IImGuiObjectExplorerService
    {
        private readonly DefaultImGuiObjectExplorer _defaultExplorer;
        private readonly ImGuiObjectExplorer[] _explorers;
        private readonly Dictionary<Type, ImGuiObjectExplorer> _typeExplorers;

        public ImGuiObjectExplorerService(DefaultImGuiObjectExplorer defaultExplorer, IEnumerable<ImGuiObjectExplorer> explorers)
        {
            this._explorers = [.. explorers.OrderBy(x => x.Priority)];
            this._typeExplorers = [];
            this._defaultExplorer = defaultExplorer;

            foreach (var explorer in this._explorers)
            {
                explorer.explorer = this;
            }
        }

        public TextFilterResultEnum DrawObjectExplorer(object instance, string filter = "", int maxDepth = 5, HashSet<object>? tree = null) => this.DrawObjectExplorer(null, null, instance.GetType(), instance, filter, maxDepth, 0, tree ?? []);

        public TextFilterResultEnum DrawObjectExplorer(int? index, string? name, Type type, object? instance, string filter, int maxDepth, int currentDepth, HashSet<object> tree)
        {
            if (currentDepth >= maxDepth)
            {
                return this._defaultExplorer.DrawObjectExplorer(index, name, type, instance, filter, maxDepth, currentDepth, tree);
            }

            return this.GetObjectExplorer(type).DrawObjectExplorer(index, name, type, instance, filter, maxDepth, currentDepth, tree);
        }

        private ImGuiObjectExplorer GetObjectExplorer(Type type)
        {
            ref ImGuiObjectExplorer? explorer = ref CollectionsMarshal.GetValueRefOrAddDefault(this._typeExplorers, type, out bool exists);

            if (exists)
            {
                return explorer!;
            }

            return explorer = this._explorers.First(x => x.AppliesTo(type));
        }
    }
}