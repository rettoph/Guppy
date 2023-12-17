using Guppy.Attributes;
using Guppy.Enums;
using Guppy.Files.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.ImGui
{
    public abstract class ImGuiObjectViewer
    {
        internal ImGuiObjectViewer()
        {
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

        public abstract bool AppliesTo(Type type);
        public virtual bool RenderObjectViewer(int? index, string? name, Type type, object? instance, IImGui imgui, string? filter, int maxDepth, int currentDepth)
        {
            string title = this.GetTitle(index, name, type, instance);

            if(currentDepth >= maxDepth)
            {
                imgui.Text(title);
                return filter.IsNullOrEmpty() || title.Contains(filter!);
            }

            return this.RenderObjectViewer(title, type, instance, imgui, filter, maxDepth, currentDepth);
        }
        protected abstract bool RenderObjectViewer(string title, Type type, object? instance, IImGui imgui, string? filter, int maxDepth, int currentDepth);
    }

    [Service<ImGuiObjectViewer>(ServiceLifetime.Singleton, true)]
    public abstract class ImGuiObjectViewer<T> : ImGuiObjectViewer
    {
        public ImGuiObjectViewer() : base()
        {
        }

        public override bool AppliesTo(Type type)
        {
            return type.IsAssignableTo(typeof(T));
        }

        protected override bool RenderObjectViewer(string title, Type type, object? instance, IImGui imgui, string? filter, int maxDepth, int currentDepth)
        {
            return this.RenderObjectViewer(title, type, instance is null ? default : (T?)instance, imgui, filter, maxDepth, currentDepth);
        }

        protected abstract bool RenderObjectViewer(string title, Type type, T? instance, IImGui imgui, string? filter, int maxDepth, int currentDepth);
    }
}
