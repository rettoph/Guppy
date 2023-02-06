using Guppy.MonoGame.UI.Definitions;
using Guppy.MonoGame.UI.Resources;
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
        private ImGuiIOPtr _io;
        private IResourceProvider _resources;
        private Dictionary<string, ImGuiFont> _fonts;

        public unsafe ImGuiFontProvider(
            IResourceProvider resources,
            ImGuiIOPtr io)
        {
            _resources = resources;
            _io = io;

            _fonts = new Dictionary<string, ImGuiFont>();
        }

        public ImGuiFont this[string name] => _fonts[name];

        public void Add(params string[] names)
        {
            foreach(string name in names)
            {
                var font = _resources.Get<ImGuiFontResource.ImGuiFontFactory>(name).Value(_io);
                _fonts.Add(name, font);
            }
        }

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
