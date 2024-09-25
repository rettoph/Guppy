using Guppy.Core.Common.Helpers;
using System.Reflection;

namespace Guppy.Tests.Core
{
    public class DelegateHelper_Tests
    {
        delegate object TestDelegate_Object_Int32(int param);
        private static object TestDelegate_Object_Int32__StaticFunction_Object_Int32(int param) => IncrementInvocationCount(nameof(TestDelegate_Object_Int32__StaticFunction_Object_Int32));
        private object TestDelegate_Object_Int32__InstanceFunction_Object_Int32(int param) => IncrementInvocationCount(nameof(TestDelegate_Object_Int32__InstanceFunction_Object_Int32));
        private int TestDelegate_Object_Int32__InstanceFunction_Int32_Int32(int param) => IncrementInvocationCount(nameof(TestDelegate_Object_Int32__InstanceFunction_Int32_Int32));
        private object TestDelegate_Object_Int32__InstanceFunction_Object_Object(object param) => IncrementInvocationCount(nameof(TestDelegate_Object_Int32__InstanceFunction_Object_Object));
        private object TestDelegate_Object_Int32__InstanceFunction_Object_String(string param) => IncrementInvocationCount(nameof(TestDelegate_Object_Int32__InstanceFunction_Object_String));

        [Theory]
        [InlineData(nameof(TestDelegate_Object_Int32__StaticFunction_Object_Int32), BindingFlags.Static | BindingFlags.NonPublic, DelegateHelper.IsCompatibleResultEnum.Compatible)]
        [InlineData(nameof(TestDelegate_Object_Int32__InstanceFunction_Object_Int32), BindingFlags.Instance | BindingFlags.NonPublic, DelegateHelper.IsCompatibleResultEnum.Compatible)]
        [InlineData(nameof(TestDelegate_Object_Int32__InstanceFunction_Int32_Int32), BindingFlags.Instance | BindingFlags.NonPublic, DelegateHelper.IsCompatibleResultEnum.CompatibleWithCasting)]
        [InlineData(nameof(TestDelegate_Object_Int32__InstanceFunction_Object_Object), BindingFlags.Instance | BindingFlags.NonPublic, DelegateHelper.IsCompatibleResultEnum.CompatibleWithCasting)]
        [InlineData(nameof(TestDelegate_Object_Int32__InstanceFunction_Object_String), BindingFlags.Instance | BindingFlags.NonPublic, DelegateHelper.IsCompatibleResultEnum.Incompatible)]
        public void TestDelegate_Object_Int32__IsCompatible(string methodName, BindingFlags bindingFlags, DelegateHelper.IsCompatibleResultEnum expected)
        {
            MethodInfo method = typeof(DelegateHelper_Tests).GetMethod(methodName, bindingFlags)!;

            bool isCompatible = DelegateHelper.IsCompatible<TestDelegate_Object_Int32>(method, out var result);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(nameof(TestDelegate_Object_Int32__StaticFunction_Object_Int32), BindingFlags.Static | BindingFlags.NonPublic)]
        [InlineData(nameof(TestDelegate_Object_Int32__InstanceFunction_Object_Int32), BindingFlags.Instance | BindingFlags.NonPublic)]
        [InlineData(nameof(TestDelegate_Object_Int32__InstanceFunction_Int32_Int32), BindingFlags.Instance | BindingFlags.NonPublic)]
        [InlineData(nameof(TestDelegate_Object_Int32__InstanceFunction_Object_Object), BindingFlags.Instance | BindingFlags.NonPublic)]
        public void TestDelegate_Object_Int32__Invocation(string methodName, BindingFlags bindingFlags)
        {
            MethodInfo method = typeof(DelegateHelper_Tests).GetMethod(methodName, bindingFlags)!;

            _invocations.Clear();

            DelegateHelper.CreateDelegate<TestDelegate_Object_Int32>(method, this).Invoke(42);

            Assert.Equal(1, _invocations[methodName]);
        }

        delegate object TestDelegate_Object_Int32_Int32(int param1, int param2);
        private object TestDelegate_Object_Int32_Int32__InstanceFunction_Object_Int32_Int32(int param1, int param2) => IncrementInvocationCount(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Object_Int32_Int32));
        private int TestDelegate_Object_Int32_Int32__InstanceFunction_Int32_Int32_Int32(int param1, int param2) => IncrementInvocationCount(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Int32_Int32_Int32));
        private object TestDelegate_Object_Int32_Int32__InstanceFunction_Object_Object_Int32(object param1, int param2) => IncrementInvocationCount(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Object_Object_Int32));
        private object TestDelegate_Object_Int32_Int32__InstanceFunction_Object_String_Int32(string param1, int param2) => IncrementInvocationCount(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Object_String_Int32));

        [Theory]
        [InlineData(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Object_Int32_Int32), BindingFlags.Instance | BindingFlags.NonPublic, DelegateHelper.IsCompatibleResultEnum.Compatible)]
        [InlineData(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Int32_Int32_Int32), BindingFlags.Instance | BindingFlags.NonPublic, DelegateHelper.IsCompatibleResultEnum.CompatibleWithCasting)]
        [InlineData(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Object_Object_Int32), BindingFlags.Instance | BindingFlags.NonPublic, DelegateHelper.IsCompatibleResultEnum.CompatibleWithCasting)]
        [InlineData(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Object_String_Int32), BindingFlags.Instance | BindingFlags.NonPublic, DelegateHelper.IsCompatibleResultEnum.Incompatible)]
        public void TestDelegate_Object_Int32_Int32__IsCompatible(string methodName, BindingFlags bindingFlags, DelegateHelper.IsCompatibleResultEnum expected)
        {
            MethodInfo method = typeof(DelegateHelper_Tests).GetMethod(methodName, bindingFlags)!;

            bool isCompatible = DelegateHelper.IsCompatible<TestDelegate_Object_Int32_Int32>(method, out var result);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Object_Int32_Int32), BindingFlags.Instance | BindingFlags.NonPublic)]
        [InlineData(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Int32_Int32_Int32), BindingFlags.Instance | BindingFlags.NonPublic)]
        [InlineData(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Object_Object_Int32), BindingFlags.Instance | BindingFlags.NonPublic)]
        public void TestDelegate_Object_Int32_Int32__Invocation(string methodName, BindingFlags bindingFlags)
        {
            MethodInfo method = typeof(DelegateHelper_Tests).GetMethod(methodName, bindingFlags)!;

            _invocations.Clear();

            DelegateHelper.CreateDelegate<TestDelegate_Object_Int32_Int32>(method, this).Invoke(42, 69);

            Assert.Equal(1, _invocations[methodName]);
        }

        delegate void TestDelegate_Void_Int32(int param);
        private static void TestDelegate_Void_Int32__StaticFunction_Void_Int32(int param) => IncrementInvocationCount(nameof(TestDelegate_Void_Int32__StaticFunction_Void_Int32));
        private void TestDelegate_Void_Int32__InstanceFunction_Void_Int32(int param) => IncrementInvocationCount(nameof(TestDelegate_Void_Int32__InstanceFunction_Void_Int32));
        private void TestDelegate_Void_Int32__InstanceFunction_Void_Object(object param) => IncrementInvocationCount(nameof(TestDelegate_Void_Int32__InstanceFunction_Void_Object));
        private void TestDelegate_Void_Int32__InstanceFunction_Void_String(string param) => IncrementInvocationCount(nameof(TestDelegate_Void_Int32__InstanceFunction_Void_String));

        [Theory]
        [InlineData(nameof(TestDelegate_Void_Int32__StaticFunction_Void_Int32), BindingFlags.Static | BindingFlags.NonPublic, DelegateHelper.IsCompatibleResultEnum.Compatible)]
        [InlineData(nameof(TestDelegate_Void_Int32__InstanceFunction_Void_Int32), BindingFlags.Instance | BindingFlags.NonPublic, DelegateHelper.IsCompatibleResultEnum.Compatible)]
        [InlineData(nameof(TestDelegate_Void_Int32__InstanceFunction_Void_Object), BindingFlags.Instance | BindingFlags.NonPublic, DelegateHelper.IsCompatibleResultEnum.CompatibleWithCasting)]
        [InlineData(nameof(TestDelegate_Void_Int32__InstanceFunction_Void_String), BindingFlags.Instance | BindingFlags.NonPublic, DelegateHelper.IsCompatibleResultEnum.Incompatible)]
        public void TestDelegate_Void_Int32__IsCompatible(string methodName, BindingFlags bindingFlags, DelegateHelper.IsCompatibleResultEnum expected)
        {
            MethodInfo method = typeof(DelegateHelper_Tests).GetMethod(methodName, bindingFlags)!;

            bool isCompatible = DelegateHelper.IsCompatible<TestDelegate_Void_Int32>(method, out var result);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(nameof(TestDelegate_Void_Int32__StaticFunction_Void_Int32), BindingFlags.Static | BindingFlags.NonPublic)]
        [InlineData(nameof(TestDelegate_Void_Int32__InstanceFunction_Void_Int32), BindingFlags.Instance | BindingFlags.NonPublic)]
        [InlineData(nameof(TestDelegate_Void_Int32__InstanceFunction_Void_Object), BindingFlags.Instance | BindingFlags.NonPublic)]
        public void TestDelegate_Void_Int32__Invocation(string methodName, BindingFlags bindingFlags)
        {
            MethodInfo method = typeof(DelegateHelper_Tests).GetMethod(methodName, bindingFlags)!;

            _invocations.Clear();

            DelegateHelper.CreateDelegate<TestDelegate_Void_Int32>(method, this).Invoke(42);

            Assert.Equal(1, _invocations[methodName]);
        }

        private static Dictionary<string, int> _invocations = new Dictionary<string, int>();
        private static int IncrementInvocationCount(string method)
        {
            if (_invocations.ContainsKey(method))
            {
                return _invocations[method]++;
            }

            return _invocations[method] = 1;
        }
        private static int GetInvocationCount(string method)
        {
            if (_invocations.ContainsKey(method))
            {
                return _invocations[method];
            }

            return 0;
        }

    }
}
