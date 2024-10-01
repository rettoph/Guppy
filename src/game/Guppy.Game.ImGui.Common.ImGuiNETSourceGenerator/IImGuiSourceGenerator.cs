using Guppy.Game.ImGui.Common.ImGuiNETSourceGenerator.TypeManagers;
using Microsoft.CodeAnalysis;

namespace Guppy.Game.ImGui.Common.ImGuiNETSourceGenerator
{
    [Generator]
    internal class IImGuiSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            CodeBuilder source = new CodeBuilder(ref context, context.Compilation.AssemblyName);

            using (source.File("IImGui.g.cs"))
            {
                using (source.Section("public partial interface IImGui"))
                {
                    TypeDecoratorHelper.AddStaticMethodDecorations(typeof(ImGuiNET.ImGui), source);
                }
            }

            TypeManager.GenerateAllSourceFiles(source);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // System.Diagnostics.Debugger.Launch();
        }
    }
}
