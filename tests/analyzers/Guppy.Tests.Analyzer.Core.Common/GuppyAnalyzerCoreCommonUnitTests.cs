using Guppy.Analyzer.Core.Common;
using Guppy.Core.Common.Attributes;
using Microsoft.CodeAnalysis;
using VerifyCS = Guppy.Tests.Analyzer.Core.Common.CSharpCodeFixVerifier<
    Guppy.Analyzer.Core.Common.RequireSequenceGroupAnalyzer,
    Microsoft.CodeAnalysis.Testing.EmptyCodeFixProvider>;

namespace Guppy.Tests.Analyzer.Core.Common
{
    public class GuppyAnalyzerCoreCommonUnitTest
    {
        [Fact]
        public async Task RequireSequenceGroup_AttributeMissingOnInterfaceImplementation()
        {
            string test = @"
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

            var expected = VerifyCS.Diagnostic(RequireSequenceGroupAnalyzer.DiagnosticId).WithSpan(25, 21, 25, 33).WithArguments("SomeFunction", "TestClass", "ConsoleApplication1.TestSequenceGroup");
            await VerifyCS.VerifyCodeFixAsync(
                source: test,
                expected: [
                    expected
                ],
                fixedSource: null,
                additionalReferences: [
                    MetadataReference.CreateFromFile(typeof(RequireSequenceGroupAttribute<>).Assembly.Location),
                ]);
        }

        [Fact]
        public async Task RequireSequenceGroup_AttributeMissingOnAbstractImplementation()
        {
            string test = @"
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

    abstract class BaseTestClass : ITestInterface
    {
        public abstract void SomeFunction();
    }

    class TestClass : BaseTestClass
    {   
        public override void SomeFunction()
        {
            throw new NotImplementedException();
        }
    }
}";

            var expected = VerifyCS.Diagnostic(RequireSequenceGroupAnalyzer.DiagnosticId).WithSpan(30, 30, 30, 42).WithArguments("SomeFunction", "TestClass", "ConsoleApplication1.TestSequenceGroup");
            await VerifyCS.VerifyCodeFixAsync(
                source: test,
                expected: [
                    expected
                ],
                fixedSource: null,
                additionalReferences: [
                    MetadataReference.CreateFromFile(typeof(RequireSequenceGroupAttribute<>).Assembly.Location),
                ]);
        }

        [Fact]
        public async Task RequireSequenceGroup_IRuntimeSequenceGroupImplementation()
        {
            string test = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Interfaces;
using Guppy.Core.Common;

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

    abstract class BaseTestClass : ITestInterface, IRuntimeSequenceGroup<TestSequenceGroup>
    {
        public SequenceGroup<TestSequenceGroup> Value { get; } = default!;

        public abstract void SomeFunction();
    }

    class TestClass : BaseTestClass
    {   
        public override void SomeFunction()
        {
            throw new NotImplementedException();
        }
    }
}";

            await VerifyCS.VerifyCodeFixAsync(
                source: test,
                expected: [],
                fixedSource: null,
                additionalReferences: [
                    MetadataReference.CreateFromFile(typeof(RequireSequenceGroupAttribute<>).Assembly.Location),
                ]);
        }
    }
}