using Guppy.GUI.Styling;
using Guppy.Resources;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Guppy.GUI.Services
{
    internal partial class ImGuiService : IImGuiService
    {
        private readonly Dictionary<Resource<Style>, IStyler> _stylers;
        private readonly IResourceProvider _resources;
        private readonly ImGuiBatch _batch;

        public ImGuiService(IResourceProvider resources, ImGuiBatch batch)
        {
            _stylers = new Dictionary<Resource<Style>, IStyler>();
            _resources = resources;
            _batch = batch;
        }

        public ImGuiFont GetFont(Resource<TrueTypeFont> ttf, int size)
        {
            if (_batch.Running)
            {
                throw new InvalidOperationException();
            }

            return _batch.GetFont(ttf, size);
        }

        public IStyler GetStyler(Resource<Style> style)
        {
            if (_batch.Running)
            {
                throw new InvalidOperationException();
            }

            ref IStyler? styler = ref CollectionsMarshal.GetValueRefOrAddDefault(_stylers, style, out bool exists);
            if (exists)
            {
                return styler!;
            }

            styler = _resources.Get(style).BuildStyler(this, _resources);

            return styler;
        }
    }
}
