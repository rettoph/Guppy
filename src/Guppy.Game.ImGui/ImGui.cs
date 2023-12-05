using Guppy.Common;
using Guppy.Game.ImGui.Styling;
using Guppy.Resources;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Guppy.Game.ImGui
{
    internal partial class ImGui : IImGui
    {
        private readonly Dictionary<Resource<ImStyle>, ResourceValue<ImStyle>> _styles;
        private readonly IResourceProvider _resources;
        private readonly IImguiBatch _batch;

        public ImGui(IResourceProvider resources, IImguiBatch batch)
        {
            _styles = new Dictionary<Resource<ImStyle>, ResourceValue<ImStyle>>();
            _resources = resources;
            _batch = batch;
        }

        public Ref<ImFontPtr> GetFont(Resource<TrueTypeFont> ttf, int size)
        {
            if (_batch.Running)
            {
                throw new InvalidOperationException();
            }

            return _batch.GetFont(ttf, size);
        }

        public ResourceValue<ImStyle> GetStyle(Resource<ImStyle> style)
        {
            if (_batch.Running)
            {
                throw new InvalidOperationException();
            }

            ref ResourceValue<ImStyle>? styleValue = ref CollectionsMarshal.GetValueRefOrAddDefault(_styles, style, out bool exists);
            if (exists)
            {
                return styleValue!;
            }

            styleValue = _resources.Get(style);

            return styleValue;
        }
    }
}
