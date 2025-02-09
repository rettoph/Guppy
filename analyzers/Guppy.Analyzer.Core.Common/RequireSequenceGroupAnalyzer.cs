using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Guppy.Analyzer.Core.Common
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RequireSequenceGroupAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Guppy_RequireSequenceGroup";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString _title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString _messageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString _description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string _category = "Design";

#pragma warning disable RS1033 // Define diagnostic description correctly
        private static readonly DiagnosticDescriptor _rule = new DiagnosticDescriptor(DiagnosticId, _title, _messageFormat, _category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: _description);
#pragma warning restore RS1033 // Define diagnostic description correctly

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(_rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }


        private const string _sequenceGroupAttribute = "Guppy.Core.Common.Attributes.SequenceGroupAttribute";
        private const string _requireSequenceGroupAttribute = "Guppy.Core.Common.Attributes.RequireSequenceGroupAttribute";
        private const string _requireGenericSequenceGroupAttribute = "Guppy.Core.Common.Attributes.RequireGenericSequenceGroupAttribute";

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
            if (methodSymbol.IsVirtual == true && containingTypeSymbol.IsAbstract == true)
            {
                return;
            }
            if (methodSymbol.IsOverride == true && containingTypeSymbol.IsAbstract == true)
            {
                return;
            }

            List<string> namedSequenceGroups = new List<string>();
            List<string> requiredSequenceGroups = new List<string>();

            IMethodSymbol methodImplementationSymbol = methodSymbol;
            while (methodImplementationSymbol != null)
            {
                AddMatchingAttributeTypeArgumentNames(namedSequenceGroups, methodImplementationSymbol, methodImplementationSymbol.GetAttributes(), _sequenceGroupAttribute);
                methodImplementationSymbol = methodImplementationSymbol.OverriddenMethod;
            }


            ImmutableArray<INamedTypeSymbol> interfaceSymbols = containingTypeSymbol.AllInterfaces;
            foreach (INamedTypeSymbol interfaceSymbol in interfaceSymbols)
            {
                if (interfaceSymbol.Name == "IRuntimeSequenceGroup" && interfaceSymbol.ContainingNamespace.ToString() == "Guppy.Core.Common.Interfaces")
                {
                    ITypeSymbol runtimeTypeSymbol = interfaceSymbol.TypeArguments[0];
                    string runtimeSequenceGroup = $"{runtimeTypeSymbol.ContainingNamespace}.{runtimeTypeSymbol.Name}";
                    namedSequenceGroups.Add(runtimeSequenceGroup);
                }

                foreach (ISymbol interfaceMemberSymbol in interfaceSymbol.GetMembers(methodSymbol.Name))
                {
                    if (interfaceMemberSymbol is IMethodSymbol interfaceMethodSymbol == false)
                    {
                        continue;
                    }

                    IMethodSymbol implementationSymbol = (IMethodSymbol)containingTypeSymbol.FindImplementationForInterfaceMember(interfaceMethodSymbol);

                    if (implementationSymbol == null)
                    {
                        continue;
                    }

                    if (IsEqualOrOverride(methodSymbol, implementationSymbol) == false)
                    {
                        continue;
                    }

                    ImmutableArray<AttributeData> interfaceMemberAttributes = interfaceMemberSymbol.GetAttributes();
                    AddMatchingAttributeTypeArgumentNames(namedSequenceGroups, (IMethodSymbol)interfaceMemberSymbol, interfaceMemberAttributes, _sequenceGroupAttribute);
                    AddMatchingAttributeTypeArgumentNames(requiredSequenceGroups, (IMethodSymbol)interfaceMemberSymbol, interfaceMemberAttributes, _requireSequenceGroupAttribute);
                    AddMatchingAttributeTypeArgumentNames(requiredSequenceGroups, (IMethodSymbol)interfaceMemberSymbol, interfaceMemberAttributes, _requireGenericSequenceGroupAttribute);
                }
            }

            foreach (string missingSequenceGroup in requiredSequenceGroups.Except(namedSequenceGroups))
            {
                // For all such symbols, produce a diagnostic.
                var diagnostic = Diagnostic.Create(_rule, methodSymbol.Locations[0], methodSymbol.Name, containingTypeSymbol.Name, missingSequenceGroup);

                context.ReportDiagnostic(diagnostic);
            }
        }

        private static void AddMatchingAttributeTypeArgumentNames(
            List<string> names,
            IMethodSymbol containingMethodSymbol,
            ImmutableArray<AttributeData> attributes,
            string attributeTypeName)
        {
            foreach (AttributeData attributeData in attributes)
            {
                if ($"{attributeData.AttributeClass.ContainingNamespace}.{attributeData.AttributeClass.Name}" != attributeTypeName)
                {
                    continue;
                }


                ITypeSymbol genericType = null;
                if (attributeData.AttributeClass.IsGenericType == true)
                {
                    genericType = attributeData.AttributeClass.TypeArguments[0];
                }

                if (attributeData.AttributeClass.IsGenericType == false
                    && attributeData.ConstructorArguments.Length == 1
                    && attributeData.ConstructorArguments[0].Type.Name == "String"
                    && containingMethodSymbol.ContainingType.IsGenericType)
                {
                    string genericTypeName = (string)attributeData.ConstructorArguments[0].Value;
                    for (int i = 0; i < containingMethodSymbol.ContainingType.TypeParameters.Length; i++)
                    {
                        if (containingMethodSymbol.ContainingType.TypeParameters[i].Name == genericTypeName)
                        {
                            genericType = containingMethodSymbol.ContainingType.TypeArguments[i];
                        }
                    }
                }


                if (genericType is null)
                {
                    continue;
                }

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