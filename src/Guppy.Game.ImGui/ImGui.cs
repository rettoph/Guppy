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
    internal partial class ImGui : IImGui, IDisposable
    {
        private readonly ImGuiObjectViewer[] _objectViewers;
        private readonly Dictionary<Resource<ImStyle>, ResourceValue<ImStyle>> _styles;
        private readonly IResourceProvider _resources;
        private readonly IImguiBatch _batch;
        private readonly Stack<ImStyle> _styleStack;

        public ImGui(IResourceProvider resources, IImguiBatch batch, DefaultImGuiObjectViewer defaultObjectViewer, IEnumerable<ImGuiObjectViewer> objectViewers)
        {
            _objectViewers = objectViewers.Concat(defaultObjectViewer.Yield()).ToArray();
            _styles = new Dictionary<Resource<ImStyle>, ResourceValue<ImStyle>>();
            _resources = resources;
            _batch = batch;
            _styleStack = new Stack<ImStyle>();
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

        public bool ObjectViewer(object instance, string filter = "", int maxDepth = 5, int currentDepth = 0)
        {
            return this.ObjectViewer(null, null, instance.GetType(), instance, filter, maxDepth, currentDepth);
        }

        public bool ObjectViewer(int? index, string? name, Type type, object? instance, string filter, int maxDepth, int currentDepth)
        {
            return _objectViewers.First(x => x.AppliesTo(type)).RenderObjectViewer(index, name, type, instance, this, filter, maxDepth, currentDepth);
        }
    }
}
