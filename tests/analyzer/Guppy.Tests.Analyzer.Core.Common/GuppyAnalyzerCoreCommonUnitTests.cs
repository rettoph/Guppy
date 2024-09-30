using Guppy.Core.Common.Attributes;
using Microsoft.CodeAnalysis;
using VerifyCS = Guppy.Tests.Analyzer.Core.Common.CSharpCodeFixVerifier<
    Guppy.Analyzer.Core.Common.GuppyAnalyzerCoreCommonAnalyzer,
    Microsoft.CodeAnalysis.Testing.EmptyCodeFixProvider>;

namespace Guppy.Tests.Analyzer.Core.Common
{
    public class GuppyAnalyzerCoreCommonUnitTest
    {
        //Diagnostic and CodeFix both triggered and checked for
        [Fact]
        public async Task TestMethod2()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;
    using Guppy.Core.Common.Attributes;

    namespace ConsoleApplication1
    {
        enum TestSequenceGroup
        {
            Default
        }

        interface ITestInterface
        {
            [RequireSequenceGroup<TestSequenceGroup>]
            void SomeFunction();
        }

        class TestClass : ITestInterface
        {   
            public void SomeFunction()
            {
                throw new NotImplementedException();
            }
        }
    }";

            var runtimeDirectory = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();

            var expected = VerifyCS.Diagnostic("GuppyAnalyzerCoreCommon").WithArguments("SomeFunction");
            await VerifyCS.VerifyCodeFixAsync(
                source: test,
                expected: [
                    expected
                ],
                fixedSource: string.Empty,
                additionalReferences: [
                    MetadataReference.CreateFromFile(typeof(RequireSequenceGroupAttribute<>).Assembly.Location),
                ]);
        }
    }
}
