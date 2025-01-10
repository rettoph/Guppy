using System;

namespace Guppy.Game.ImGui.Common.ImGuiNETSourceGenerator.TypeManagers
{
    internal class EnumTypeManager : TypeManager
    {
        public EnumTypeManager(Type imGuiType) : base(imGuiType, imGuiType.Name.ToGuppyName())
        {
        }

        protected override void InternalGenerateSourceFiles(CodeBuilder source)
        {
            using (source.File($"{this.GuppyType}.g.cs"))
            {
                using (source.Section($"public enum {this.GuppyType}"))
                {
                    string[] names = Enum.GetNames(this.ImGuiType);
                    foreach (string name in names)
                    {
                        Enum value = (Enum)Enum.Parse(this.ImGuiType, name);
                        object numericalValue = Convert.ChangeType(value, value.GetTypeCode());
                        source.AppendLine($"{name} = {numericalValue},");
                    }
                }

                using (source.Section($"internal static class {this.GuppyType}Converter"))
                {
                    using (source.Section($"public static {this.ImGuiType.FullName} ConvertToImGui({this.GuppyType} value)"))
                    {
                        source.AppendLine($"return System.Runtime.CompilerServices.Unsafe.As<{this.GuppyType}, {this.ImGuiType.FullName}>(ref value);");
                    }

                    using (source.Section($"public static {this.GuppyType} ConvertToGuppy({this.ImGuiType.FullName} value)"))
                    {
                        source.AppendLine($"return System.Runtime.CompilerServices.Unsafe.As<{this.ImGuiType.FullName}, {this.GuppyType}>(ref value);");
                    }
                }
            }
        }

        public override string GetGuppyToImGuiConverter(string parameter)
        {
            return $"{this.GuppyType}Converter.ConvertToImGui({parameter})";
        }

        public override string GetImGuiToGuppyConverter(string parameter)
        {
            return $"{this.GuppyType}Converter.ConvertToGuppy({parameter})";
        }
    }
}