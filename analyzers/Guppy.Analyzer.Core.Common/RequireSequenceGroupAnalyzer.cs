using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Guppy.Analyzer.Core.Common
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RequireSequenceGroupAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Guppy_RequireSequenceGroup";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Design";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }


        private static string SequenceGroupAttribute = "Guppy.Core.Common.Attributes.SequenceGroupAttribute";
        private static string RequireSequenceGroupAttribute = "Guppy.Core.Common.Attributes.RequireSequenceGroupAttribute";

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            IMethodSymbol methodSymbol = (IMethodSymbol)context.Symbol;
            ITypeSymbol containingTypeSymbol = methodSymbol.ContainingType;
            if (containingTypeSymbol.TypeKind == TypeKind.Interface)
            {
                return;
            }
            if (methodSymbol.IsAbstract == true)
            {
                return;
            }

            List<string> namedSequenceGroups = new List<string>();
            List<string> requiredSequenceGroups = new List<string>();

            IMethodSymbol methodImplementationSymbol = methodSymbol;
            while (methodImplementationSymbol != null)
            {
                AddMatchingAttributeTypeArgumentNames(namedSequenceGroups, methodImplementationSymbol.GetAttributes(), SequenceGroupAttribute);
                methodImplementationSymbol = methodImplementationSymbol.OverriddenMethod;
            }


            ImmutableArray<INamedTypeSymbol> interfaceSymbols = containingTypeSymbol.AllInterfaces;
            foreach (INamedTypeSymbol interfaceSymbol in interfaceSymbols)
            {
                foreach (ISymbol interfaceMemberSymbol in interfaceSymbol.GetMembers())
                {
                    IMethodSymbol implementationSymbol = (IMethodSymbol)containingTypeSymbol.FindImplementationForInterfaceMember(interfaceMemberSymbol);

                    if (implementationSymbol == null)
                    {
                        continue;
                    }

                    if (IsEqualOrOverride(methodSymbol, implementationSymbol) == false)
                    {
                        continue;
                    }

                    ImmutableArray<AttributeData> interfaceMemberAttributes = interfaceMemberSymbol.GetAttributes();
                    AddMatchingAttributeTypeArgumentNames(namedSequenceGroups, interfaceMemberAttributes, SequenceGroupAttribute);
                    AddMatchingAttributeTypeArgumentNames(requiredSequenceGroups, interfaceMemberAttributes, RequireSequenceGroupAttribute);
                }
            }

            foreach (string missingSequenceGroup in requiredSequenceGroups.Except(namedSequenceGroups))
            {
                // For all such symbols, produce a diagnostic.
                var diagnostic = Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name, containingTypeSymbol.Name, missingSequenceGroup);

                context.ReportDiagnostic(diagnostic);
            }
        }

        private static void AddMatchingAttributeTypeArgumentNames(
            List<string> names,
            ImmutableArray<AttributeData> attributes,
            string attributeTypeName)
        {
            foreach (AttributeData attributeData in attributes)
            {
                if (attributeData.AttributeClass.IsGenericType == false)
                {
                    continue;
                }

                if ($"{attributeData.AttributeClass.ContainingNamespace}.{attributeData.AttributeClass.Name}" != attributeTypeName)
                {
                    continue;
                }

                ITypeSymbol genericType = attributeData.AttributeClass.TypeArguments[0];
                names.Add($"{genericType.ContainingNamespace}.{genericType.Name}");
            }
        }

        private static bool IsEqualOrOverride(IMethodSymbol overrideMethodSymbol, IMethodSymbol baseMethodSymbol)
        {
            while (overrideMethodSymbol != null)
            {
                if (SymbolEqualityComparer.Default.Equals(overrideMethodSymbol, baseMethodSymbol) == true)
                {
                    return true;
                }

                if (overrideMethodSymbol.IsOverride == false)
                {
                    return false;
                }

                overrideMethodSymbol = overrideMethodSymbol.OverriddenMethod;
            }


            return false;
        }
    }
}