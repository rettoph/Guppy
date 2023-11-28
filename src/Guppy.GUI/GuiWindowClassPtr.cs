using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    [StructLayout(LayoutKind.Auto)]
    public unsafe partial struct GuiWindowClassPtr
    {
        public ref uint ClassId => ref this.Value.ClassId;
        public ref uint ParentViewportId => ref this.Value.ParentViewportId;
        public int ViewportFlagsOverrideSet => (int)this.Value.ViewportFlagsOverrideSet;
        public int ViewportFlagsOverrideClear => (int)this.Value.ViewportFlagsOverrideClear;
        public GuiTabItemFlags TabItemFlagsOverrideSet
        {
            get => GuiTabItemFlagsConverter.ConvertToGuppy(this.Value.TabItemFlagsOverrideSet);
            set => this.Value.TabItemFlagsOverrideSet = GuiTabItemFlagsConverter.ConvertToImGui(value);
        }
        public GuiDockNodeFlags DockNodeFlagsOverrideSet
        {
            get => GuiDockNodeFlagsConverter.ConvertToGuppy(this.Value.DockNodeFlagsOverrideSet);
            set => this.Value.DockNodeFlagsOverrideSet = GuiDockNodeFlagsConverter.ConvertToImGui(value);
        }

        public ref bool DockingAlwaysTabBar => ref this.Value.DockingAlwaysTabBar;
        public ref bool DockingAllowUnclassed => ref this.Value.DockingAllowUnclassed;

        public GuiWindowClassPtr() : this(new ImGuiWindowClassPtr(ImGuiNative.ImGuiWindowClass_ImGuiWindowClass()))
        {
        }
    }
}
