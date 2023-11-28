using Guppy.GUI.Styling;
using Guppy.Resources;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Guppy.GUI
{
    internal partial class Gui : IGui
    {
        private readonly Dictionary<Resource<Style>, IGuiStyle> _stylers;
        private readonly IResourceProvider _resources;
        private readonly ImGuiBatch _batch;

        public Gui(IResourceProvider resources, ImGuiBatch batch)
        {
            _stylers = new Dictionary<Resource<Style>, IGuiStyle>();
            _resources = resources;
            _batch = batch;
        }

        public GuiFontPtr GetFont(Resource<TrueTypeFont> ttf, int size)
        {
            if (_batch.Running)
            {
                throw new InvalidOperationException();
            }

            return _batch.GetFont(ttf, size);
        }

        public IGuiStyle GetStyle(Resource<Style> style)
        {
            if (_batch.Running)
            {
                throw new InvalidOperationException();
            }

            ref IGuiStyle? styler = ref CollectionsMarshal.GetValueRefOrAddDefault(_stylers, style, out bool exists);
            if (exists)
            {
                return styler!;
            }

            styler = _resources.Get(style).BuildGuiStyle(this, _resources);

            return styler;
        }
    }
}
