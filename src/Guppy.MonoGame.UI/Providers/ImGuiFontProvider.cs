using Guppy.MonoGame.UI.Definitions;
using Guppy.Resources.Providers;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Providers
{
    internal sealed class ImGuiFontProvider : IImGuiFontProvider
    {
        private IResourceProvider _resources;
        private Dictionary<string, ImGuiFont> _fonts;

        public unsafe ImGuiFontProvider(
            IResourceProvider resources,
            ImGuiIOPtr io,
            IEnumerable<IImGuiFontDefinition> definitions)
        {
            _resources = resources;

            _fonts = definitions.ToDictionary(x => x.Key, x => x.BuildFont(resources, io));
        }

        public ImGuiFont this[string name] => _fonts[name];

        public ImGuiFont Get(string name)
        {
            return _fonts[name];
        }

        public bool TryGet(string name, [MaybeNullWhen(false)] out ImGuiFont font)
        {
            return _fonts.TryGetValue(name, out font);
        }
    }
}
