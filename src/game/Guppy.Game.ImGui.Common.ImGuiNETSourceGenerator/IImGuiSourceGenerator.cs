using Guppy.Game.ImGui.Common.ImGuiNETSourceGenerator.TypeManagers;
using Microsoft.CodeAnalysis;

namespace Guppy.Game.ImGui.Common.ImGuiNETSourceGenerator
{
    [Generator]
    public class IImGuiSourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            CodeBuilder source = new CodeBuilder(ref context);

            using (source.File("IImGui.g.cs"))
            {
                using (source.Section("public partial interface IImGui"))
                {
                    TypeDecoratorHelper.AddStaticMethodDecorations(typeof(ImGuiNET.ImGui), source);
                }
            }

            TypeManager.GenerateAllSourceFiles(source);
        }
    }
}