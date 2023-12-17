using Guppy.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.ImGui.Services
{
    public interface IImGuiObjectExplorerService
    {
        TextFilterResult DrawObjectExplorer(object instance, string filter = "", int maxDepth = 5);
        TextFilterResult DrawObjectExplorer(int? index, string? name, Type type, object? instance, string filter, int maxDepth, int currentDepth);
    }
}
