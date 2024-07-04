using Guppy.Tests.Common.Extensions;
using System.Reflection;
using System.Reflection.Emit;

namespace Guppy.Tests.Core
{
    public class AssemblyService_Tests
    {
        [Fact]
        public void Test1()
        {
            string dynamicAssemblyPrefix = $"{typeof(AssemblyService_Tests)}.DynamicAssembly";
            AssemblyBuilder.DefineDynamicAssembly(new AssemblyName($"{dynamicAssemblyPrefix}_1"), AssemblyBuilderAccess.Run)
                .DefineDefaultModule(moduleBuilder =>
                {
                    moduleBuilder.DefineType("ITestType", TypeAttributes.Public | TypeAttributes.Interface, typeBuilder =>
                    {
                    });
                });
        }
    }
}
