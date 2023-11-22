using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.GUI.ImGuiNETSourceGenerator.TypeManagers
{
    internal class UnsafeAsTypeManager : TypeManager
    {
        public UnsafeAsTypeManager(Type imGuiType, string guppyType) : base(imGuiType, guppyType)
        {
        }

        public override void GenerateSourceFiles(ref GeneratorExecutionContext context)
        {
            //throw new NotImplementedException();
        }

        public override string GetGuppyToImGuiConverter(string parameter)
        {
            return $"System.Runtime.CompilerServices.Unsafe.As<{GuppyType}, {ImGuiType}>(ref {parameter})";
        }

        public override string GetImGuiToGuppyConverter(string parameter)
        {
            return $"System.Runtime.CompilerServices.Unsafe.As<{ImGuiType}, {GuppyType}>(ref {parameter})";
        }
    }
}
