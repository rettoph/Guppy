using Guppy.Core.Common;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Helpers;
using Guppy.Game.ImGui.Common.Styling;

namespace Guppy.Game.ImGui.MonoGame
{
    internal partial class ImGui : IImGui, IDisposable
    {
        private readonly IImguiBatch _batch;
        private readonly Stack<ImStyle> _styleStack;
        private readonly IResourceService _resourceService;

        private readonly IDisposable _idPopper;

        public ImGui(IImguiBatch batch, IResourceService resourceService)
        {
            _batch = batch;
            _resourceService = resourceService;
            _styleStack = new Stack<ImStyle>();
            _idPopper = new ImGuiPoppers.IdPopper(this);
        }

        public IDisposable Apply(Resource<ImStyle> style)
        {
            return this.Apply(_resourceService.GetValue(style));
        }

        public IDisposable Apply(ResourceValue<ImStyle> style)
        {
            return this.Apply(style.Value);
        }

        public IDisposable Apply(ImStyle style)
        {
            style.Push();
            _styleStack.Push(style);

            return this;
        }

        public IDisposable Apply(string key)
        {
            foreach (ImStyle style in _styleStack)
            {
                if (style.TryGetValue(key, out var value))
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
            if (_styleStack.TryPop(out ImStyle? style))
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

            return _batch.GetFont(_resourceService.GetValue(ttf), size);
        }
    }
}
