using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.GUI.ImGuiNETSourceGenerator.TypeManagers
{
    internal class DecoratingTypeManager : TypeManager
    {
        public DecoratingTypeManager(Type imGuiType) : base(imGuiType, imGuiType.Name.ToGuppyName())
        {
        }

        public override void GenerateSourceFiles(CodeBuilder source)
        {
            if(this.ImGuiType.IsByRef || this.ImGuiType.IsPointer)
            {
                TypeManager.GetTypeManager(this.ImGuiType.GetElementType());
                return;
            }

            using(source.File($"{this.GuppyType}.g.cs"))
            {
                using (source.Section($"public unsafe partial {(this.ImGuiType.IsValueType ? "struct" : "class")} {this.GuppyType}"))
                {
                    source.AppendLine($"internal {this.ImGuiType.FullName} Value;");
                    source.AppendLine();

                    using (source.Section($"internal {this.GuppyType}({this.ImGuiType.FullName} value)"))
                    {
                        source.AppendLine("this.Value = value;");
                    }

                    //TypeDecoratorHelper.AddFieldDecorations(this.ImGuiType, "this.Value", source);
                    //
                    //TypeDecoratorHelper.AddPropertyDecorations(this.ImGuiType, "this.Value", source);

                    TypeDecoratorHelper.AddPublicMethodDecorations(this.ImGuiType, "this.Value", source);
                }
            }
        }

        public override string GetGuppyToImGuiConverter(string parameter)
        {
            return $"{parameter}.Value";
        }

        public override string GetImGuiToGuppyConverter(string parameter)
        {
            return $"new {this.GuppyType.Replace("*", "")}({parameter})";
        }
    }
}
