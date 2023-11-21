﻿using ImGuiNET;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Guppy.GUI.Generator
{
    [Generator]
    internal class ImGuiEnumSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            this.BuildSource<ImGuiStyleVar>(ref context);
            this.BuildSource<ImGuiCol>(ref context);
            this.BuildSource<ImGuiWindowFlags>(ref context);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            //if (!Debugger.IsAttached) Debugger.Launch();
            //
            //Thread.Sleep(10000);
        }

        public void BuildSource<T>(ref GeneratorExecutionContext context)
            where T : unmanaged, Enum
        {
            StringBuilder source = new StringBuilder();

            source.AppendLine("// <auto-generated/>");
            source.AppendLine();

            source.AppendLine($"using System.Runtime.CompilerServices;");
            source.AppendLine($"using SourceEnum = {typeof(T).FullName};");
            source.AppendLine();

            source.AppendLine($"namespace {context.Compilation.AssemblyName}");
            source.AppendLine("{");

                source.AppendLine($"\tpublic enum {typeof(T).Name}");
                source.AppendLine("\t{");

                foreach(T value in Enum.GetValues(typeof(T)))
                {
                    T refValue = value;
                    source.AppendLine($"\t\t{value.ToString()} = {Unsafe.As<T, int>(ref refValue)},");
                }

                source.AppendLine("\t}");

                source.AppendLine($"\tinternal static class {typeof(T).Name}Converter");
                source.AppendLine("\t{");

                    source.AppendLine($"\t\tpublic static SourceEnum Convert({typeof(T).Name} value)");
                    source.AppendLine("\t\t{");

                        source.AppendLine($"\t\t\treturn Unsafe.As<{typeof(T).Name}, SourceEnum>(ref value);");

                    source.AppendLine("\t\t}");

                source.AppendLine("\t}");

            source.AppendLine("}");

            context.AddSource($"{typeof(T).Name}.g.cs", source.ToString());
        }
    }
}
