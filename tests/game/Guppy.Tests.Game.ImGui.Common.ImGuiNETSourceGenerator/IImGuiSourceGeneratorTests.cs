using Guppy.Game.ImGui.Common.ImGuiNETSourceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Guppy.Tests.Game.ImGui.Common.ImGuiNETSourceGenerator
{
    public class IImGuiSourceGeneratorTests
    {
        [Fact]
        public void IImGuiSourceGenerator_Test()
        {
            IImGuiSourceGenerator generator = new();

            CSharpCompilation compilation = CSharpCompilation.Create("CSharpCodeGen.GenerateAssembly")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator)
                .RunGeneratorsAndUpdateCompilation(compilation, out _, out var _);

            // Verify the generated code
            GeneratorDriverRunResult results = driver.GetRunResult();
        }
    }
}