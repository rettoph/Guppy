using System;

namespace Guppy.Game.ImGui.Common.ImGuiNETSourceGenerator.TypeManagers
{
    internal class UnsafeAsTypeManager : TypeManager
    {
        public UnsafeAsTypeManager(Type imGuiType, string guppyType) : base(imGuiType, guppyType)
        {
        }

        public override void GenerateSourceFiles(CodeBuilder source)
        {
            //throw new NotImplementedException();
        }

        public override string GetGuppyToImGuiConverter(string parameter)
        {
            return $"System.Runtime.CompilerServices.Unsafe.As<{this.GuppyType}, {this.ImGuiType}>(ref {parameter})";
        }

        public override string GetImGuiToGuppyConverter(string parameter)
        {
            return $"System.Runtime.CompilerServices.Unsafe.As<{this.ImGuiType}, {this.GuppyType}>(ref {parameter})";
        }
    }
}