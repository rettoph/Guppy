using Guppy.Common.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.ImGui.Services
{
    internal class ImGuiObjectExplorerService : IImGuiObjectExplorerService
    {
        private readonly IImGui _imgui;
        private readonly DefaultImGuiObjectExplorer _defaultExplorer;
        private readonly ImGuiObjectExplorer[] _explorers;
        private readonly Dictionary<Type, ImGuiObjectExplorer> _typeExplorers;

        public ImGuiObjectExplorerService(IImGui imgui, DefaultImGuiObjectExplorer defaultExplorer, IEnumerable<ImGuiObjectExplorer> explorers)
        {
            _imgui = imgui;
            _explorers = explorers.OrderBy(x => x.Priority).ToArray();
            _typeExplorers = new Dictionary<Type, ImGuiObjectExplorer>();
            _defaultExplorer = defaultExplorer;

            foreach(var explorer in _explorers)
            {
                explorer.explorer = this;
            }
        }

        public TextFilterResult DrawObjectExplorer(object instance, string filter = "", int maxDepth = 5)
        {
            return this.DrawObjectExplorer(null, null, instance.GetType(), instance, filter, maxDepth, 0);
        }

        public TextFilterResult DrawObjectExplorer(int? index, string? name, Type type, object? instance, string filter, int maxDepth, int currentDepth)
        {
            if(currentDepth >= maxDepth)
            {
                return _defaultExplorer.DrawObjectExplorer(index, name, type, instance, filter, maxDepth, currentDepth);
            }

            return this.GetObjectExplorer(type).DrawObjectExplorer(index, name, type, instance, filter, maxDepth, currentDepth);
        }

        private ImGuiObjectExplorer GetObjectExplorer(Type type)
        {
            ref ImGuiObjectExplorer? explorer = ref CollectionsMarshal.GetValueRefOrAddDefault(_typeExplorers, type, out bool exists);

            if(exists)
            {
                return explorer!;
            }

            return explorer = _explorers.First(x => x.AppliesTo(type));
        }
    }
}
