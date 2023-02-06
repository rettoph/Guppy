using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Providers
{
    public interface IImGuiFontProvider
    {
        ImGuiFont this[string name] { get; }

        void Add(params string[] names);
        ImGuiFont Get(string name);
        bool TryGet(string name, [MaybeNullWhen(false)] out ImGuiFont font);
    }
}
