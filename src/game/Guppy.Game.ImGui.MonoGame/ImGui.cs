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
            this._batch = batch;
            this._resourceService = resourceService;
            this._styleStack = new Stack<ImStyle>();
            this._idPopper = new ImGuiPoppers.IdPopper(this);
        }

        public IDisposable Apply(ResourceKey<ImStyle> style) => this.Apply(this._resourceService.Get(style));

        public IDisposable Apply(Resource<ImStyle> style) => this.Apply(style.Value);

        public IDisposable Apply(ImStyle style)
        {
            style.Push();
            this._styleStack.Push(style);

            return this;
        }

        public IDisposable Apply(string key)
        {
            foreach (ImStyle style in this._styleStack)
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

            return this._idPopper;
        }

        public void Dispose()
        {
            if (this._styleStack.TryPop(out ImStyle? style))
            {
                style.Pop();
            }
        }

        public Ref<ImFontPtr> GetFont(ResourceKey<TrueTypeFont> ttf, int size)
        {
            if (this._batch.Running)
            {
                throw new InvalidOperationException();
            }

            return this._batch.GetFont(this._resourceService.Get(ttf), size);
        }
    }
}