using VerifyCS = Guppy.Tests.Analyzer.Core.Common.CSharpCodeFixVerifier<
    Guppy.Analyzer.Core.Common.GuppyAnalyzerCoreCommonAnalyzer,
    Guppy.Analyzer.Core.Common.GuppyAnalyzerCoreCommonCodeFixProvider>;

namespace Guppy.Tests.Analyzer.Core.Common
{
    public class GuppyAnalyzerCoreCommonUnitTest
    {
        //No diagnostics expected to show up
        [Fact]
        public async Task TestMethod1()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

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

    namespace ConsoleApplication1
    {
        class {|#0:TypeName|}
        {   
            public void SomeFunction()
            {
                throw new NotImplementedException();
            }
        }
    }";

            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
        }
    }";

            var expected = VerifyCS.Diagnostic("GuppyAnalyzerCoreCommon").WithLocation(0).WithArguments("TypeName");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    }
}
