using Guppy.Common;
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
        private readonly Dictionary<Resource<Style>, ResourceValue<Style>> _styles;
        private readonly IResourceProvider _resources;
        private readonly ImGuiBatch _batch;

        public Gui(IResourceProvider resources, ImGuiBatch batch)
        {
            _styles = new Dictionary<Resource<Style>, ResourceValue<Style>>();
            _resources = resources;
            _batch = batch;
        }

        public Ref<GuiFontPtr> GetFont(Resource<TrueTypeFont> ttf, int size)
        {
            if (_batch.Running)
            {
                throw new InvalidOperationException();
            }

            return _batch.GetFont(ttf, size);
        }

        public ResourceValue<Style> GetStyle(Resource<Style> style)
        {
            if (_batch.Running)
            {
                throw new InvalidOperationException();
            }

            ref ResourceValue<Style>? styleValue = ref CollectionsMarshal.GetValueRefOrAddDefault(_styles, style, out bool exists);
            if (exists)
            {
                return styleValue!;
            }

            styleValue = _resources.Get(style);

            return styleValue;
        }
    }
}
