using Guppy.Attributes;
using Guppy.Common.Services;
using Guppy.Enums;
using Microsoft.Xna.Framework;
using System.Text;

namespace Guppy.Game.ImGui
{
    public abstract class ImGuiObjectViewer
    {
        protected readonly IObjectTextFilterService filter;

        internal ImGuiObjectViewer(IObjectTextFilterService filter)
        {
            this.filter = filter;
        }

        protected virtual string GetTitle(int? index, string? name, Type type, object? instance)
        {
            StringBuilder title = new StringBuilder();

            if (index is not null)
            {
                title.Append($"{index}: ");
            }

            title.Append($"{type.GetFormattedName()} ");

            if (name is not null)
            {
                title.Append($"{name} ");
            }

            title.Append("- ");

            if (instance is null)
            {
                title.Append("null");
            }
            else
            {
                title.Append(instance?.ToString());
            }

            return title.ToString();
        }

        private Vector4 _red = Color.Red.ToVector4();
        private Vector4 _green = Color.LightGreen.ToVector4();
        protected bool RenderTitle(IImGui imgui, string filter, string title)
        {
            if (filter.IsNullOrEmpty())
            {
                imgui.Text(title);
                return false;
            }
            else if (title.Contains(filter))
            {
                imgui.TextColored(_green, title);
                return true;
            }
            else
            {
                imgui.TextColored(_red, title);
                return false;
            }
        }

        public abstract bool AppliesTo(Type type);
        public virtual bool RenderObjectViewer(int? index, string? name, Type type, object? instance, IImGui imgui, string filter, int maxDepth, int currentDepth)
        {
            if(currentDepth >= maxDepth || instance is null)
            {
                return this.RenderTitle(imgui, filter, this.GetTitle(index, name, type, instance));
            }

            return this.InternalRenderObjectViewer(index, name, type, instance, imgui, filter, maxDepth, currentDepth);
        }
        protected abstract bool InternalRenderObjectViewer(int? index, string? name, Type type, object instance, IImGui imgui, string filter, int maxDepth, int currentDepth);
    }

    [Service<ImGuiObjectViewer>(ServiceLifetime.Singleton, true)]
    public abstract class ImGuiObjectViewer<T> : ImGuiObjectViewer
    {
        public ImGuiObjectViewer(IObjectTextFilterService filter) : base(filter)
        {
        }

        public override bool AppliesTo(Type type)
        {
            return type.IsAssignableTo(typeof(T));
        }

        protected override bool InternalRenderObjectViewer(int? index, string? name, Type type, object instance, IImGui imgui, string filter, int maxDepth, int currentDepth)
        {
            return this.RenderObjectViewer(index, name, type, instance is null ? default : (T?)instance, imgui, filter, maxDepth, currentDepth);
        }

        protected abstract bool RenderObjectViewer(int? index, string? name, Type type, T? instance, IImGui imgui, string filter, int maxDepth, int currentDepth);
    }
}
