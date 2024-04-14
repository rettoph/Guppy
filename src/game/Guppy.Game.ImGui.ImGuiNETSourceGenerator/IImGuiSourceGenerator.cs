using Guppy.GUI.ImGuiNETSourceGenerator.TypeManagers;
using ImGuiNET;
using Microsoft.CodeAnalysis;

namespace Guppy.GUI.ImGuiNETSourceGenerator
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
                    TypeDecoratorHelper.AddStaticMethodDecorations(typeof(ImGui), source);
                }
            }

            //TypeManager.GetTypeManager(typeof(ImGuiWindowClass));
            TypeManager.GenerateAllSourceFiles(source);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            //Debugger.Launch();
        }
    }
}
