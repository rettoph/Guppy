using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Guppy.Analyzer.Core.Common
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class GuppyAnalyzerCoreCommonAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "GuppyAnalyzerCoreCommon";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            IMethodSymbol methodSymbol = (IMethodSymbol)context.Symbol;

            List<AttributeData> attributeData = new List<AttributeData>();
            attributeData.AddRange(methodSymbol.GetAttributes());

            ITypeSymbol containingTypeSymbol = methodSymbol.ContainingType;

            ImmutableArray<INamedTypeSymbol> interfaceSymbols = containingTypeSymbol.AllInterfaces;

            foreach (INamedTypeSymbol interfaceSymbol in interfaceSymbols)
            {
                foreach (ISymbol interfaceMemberSymbol in interfaceSymbol.GetMembers())
                {
                    ISymbol implementationSymbol = containingTypeSymbol.FindImplementationForInterfaceMember(interfaceMemberSymbol);

                    if (implementationSymbol == null)
                    {
                        continue;
                    }

                    if (SymbolEqualityComparer.Default.Equals(implementationSymbol, methodSymbol) == false)
                    {
                        continue;
                    }

                    attributeData.AddRange(interfaceMemberSymbol.GetAttributes());
                }
            }
        }
    }
}
