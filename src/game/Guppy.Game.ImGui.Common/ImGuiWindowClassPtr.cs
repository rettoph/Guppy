using ImGuiNET;
using System.Runtime.InteropServices;

namespace Guppy.Game.ImGui.Common
{
    [StructLayout(LayoutKind.Auto)]
    public unsafe partial struct ImGuiWindowClassPtr
    {
        public ref uint ClassId => ref this.Value.ClassId;
        public ref uint ParentViewportId => ref this.Value.ParentViewportId;
        public int ViewportFlagsOverrideSet => (int)this.Value.ViewportFlagsOverrideSet;
        public int ViewportFlagsOverrideClear => (int)this.Value.ViewportFlagsOverrideClear;
        public ImGuiTabItemFlags TabItemFlagsOverrideSet
        {
            get => ImGuiTabItemFlagsConverter.ConvertToGuppy(this.Value.TabItemFlagsOverrideSet);
            set => this.Value.TabItemFlagsOverrideSet = ImGuiTabItemFlagsConverter.ConvertToImGui(value);
        }
        public ImGuiDockNodeFlags DockNodeFlagsOverrideSet
        {
            get => ImGuiDockNodeFlagsConverter.ConvertToGuppy(this.Value.DockNodeFlagsOverrideSet);
            set => this.Value.DockNodeFlagsOverrideSet = ImGuiDockNodeFlagsConverter.ConvertToImGui(value);
        }

        public ref bool DockingAlwaysTabBar => ref this.Value.DockingAlwaysTabBar;
        public ref bool DockingAllowUnclassed => ref this.Value.DockingAllowUnclassed;

        public ImGuiWindowClassPtr() : this(new ImGuiNET.ImGuiWindowClassPtr(ImGuiNative.ImGuiWindowClass_ImGuiWindowClass()))
        {
        }

        public static bool operator ==(ImGuiWindowClassPtr left, ImGuiWindowClassPtr right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ImGuiWindowClassPtr left, ImGuiWindowClassPtr right)
        {
            return !(left == right);
        }
    }
}
