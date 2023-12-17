using Guppy.Common;
using Guppy.Game.ImGui.Helpers;
using Guppy.Game.ImGui.Styling;
using Guppy.Resources;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Guppy.Game.ImGui
{
    internal partial class ImGui : IImGui, IDisposable
    {
        private readonly Dictionary<Resource<ImStyle>, ResourceValue<ImStyle>> _styles;
        private readonly IResourceProvider _resources;
        private readonly IImguiBatch _batch;
        private readonly Stack<ImStyle> _styleStack;

        private IDisposable _idPopper;

        public ImGui(IResourceProvider resources, IImguiBatch batch)
        {
            _styles = new Dictionary<Resource<ImStyle>, ResourceValue<ImStyle>>();
            _resources = resources;
            _batch = batch;
            _styleStack = new Stack<ImStyle>();
            _idPopper = new ImGuiPoppers.IdPopper(this);
        }

        public IDisposable Apply(Resource<ImStyle> style)
        {
            return this.Apply(this.GetStyle(style));
        }

        public IDisposable Apply(ImStyle style)
        {
            style.Push();
            _styleStack.Push(style);

            return this;
        }

        public IDisposable Apply(string key)
        {
            foreach(ImStyle style in _styleStack)
            {
                if(style.TryGetValue(key, out var value))
                {
                    value.Push();
                    return value;
                }
            }

            throw new KeyNotFoundException();
        }

        public IDisposable ApplyID(string id)
        {
            ((IImGui)this).PushID(id);

            return _idPopper;
        }

        public void Dispose()
        {
            if(_styleStack.TryPop(out ImStyle? style))
            {
                style.Pop();
            }
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
