using System.Reflection;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Utilities;

namespace Guppy.Tests.Core
{
    public class Delegator_Tests
    {
        private delegate object TestDelegate_Object_Int32(int param);
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable CA1859 // Use concrete types when possible for improved performance
        private static object TestDelegate_Object_Int32__StaticFunction_Object_Int32(int param)
        {
            return IncrementInvocationCount(nameof(TestDelegate_Object_Int32__StaticFunction_Object_Int32));
        }

        private object TestDelegate_Object_Int32__InstanceFunction_Object_Int32(int param)
        {
            return IncrementInvocationCount(nameof(TestDelegate_Object_Int32__InstanceFunction_Object_Int32));
        }

        private int TestDelegate_Object_Int32__InstanceFunction_Int32_Int32(int param)
        {
            return IncrementInvocationCount(nameof(TestDelegate_Object_Int32__InstanceFunction_Int32_Int32));
        }

        private object TestDelegate_Object_Int32__InstanceFunction_Object_Object(object param)
        {
            return IncrementInvocationCount(nameof(TestDelegate_Object_Int32__InstanceFunction_Object_Object));
        }

        private object TestDelegate_Object_Int32__InstanceFunction_Object_String(string param)
        {
            return IncrementInvocationCount(nameof(TestDelegate_Object_Int32__InstanceFunction_Object_String));
        }
#pragma warning restore CA1859 // Use concrete types when possible for improved performance
#pragma warning restore IDE0060 // Remove unused parameter

        [Theory]
        [InlineData(nameof(TestDelegate_Object_Int32__StaticFunction_Object_Int32), BindingFlags.Static | BindingFlags.NonPublic, DelegatorIsCompatibleResultEnum.Compatible)]
        [InlineData(nameof(TestDelegate_Object_Int32__InstanceFunction_Object_Int32), BindingFlags.Instance | BindingFlags.NonPublic, DelegatorIsCompatibleResultEnum.Compatible)]
        [InlineData(nameof(TestDelegate_Object_Int32__InstanceFunction_Int32_Int32), BindingFlags.Instance | BindingFlags.NonPublic, DelegatorIsCompatibleResultEnum.CompatibleWithCasting)]
        [InlineData(nameof(TestDelegate_Object_Int32__InstanceFunction_Object_Object), BindingFlags.Instance | BindingFlags.NonPublic, DelegatorIsCompatibleResultEnum.CompatibleWithCasting)]
        [InlineData(nameof(TestDelegate_Object_Int32__InstanceFunction_Object_String), BindingFlags.Instance | BindingFlags.NonPublic, DelegatorIsCompatibleResultEnum.Incompatible)]
        public void TestDelegate_Object_Int32__IsCompatible(string methodName, BindingFlags bindingFlags, DelegatorIsCompatibleResultEnum expected)
        {
            MethodInfo method = typeof(Delegator_Tests).GetMethod(methodName, bindingFlags)!;
            _ = Delegator<TestDelegate_Object_Int32>.IsCompatible(method, out var result);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(nameof(TestDelegate_Object_Int32__StaticFunction_Object_Int32), BindingFlags.Static | BindingFlags.NonPublic)]
        [InlineData(nameof(TestDelegate_Object_Int32__InstanceFunction_Object_Int32), BindingFlags.Instance | BindingFlags.NonPublic)]
        [InlineData(nameof(TestDelegate_Object_Int32__InstanceFunction_Int32_Int32), BindingFlags.Instance | BindingFlags.NonPublic)]
        [InlineData(nameof(TestDelegate_Object_Int32__InstanceFunction_Object_Object), BindingFlags.Instance | BindingFlags.NonPublic)]
        public void TestDelegate_Object_Int32__Invocation(string methodName, BindingFlags bindingFlags)
        {
            MethodInfo method = typeof(Delegator_Tests).GetMethod(methodName, bindingFlags)!;

            _invocations.Clear();

            Delegator<TestDelegate_Object_Int32>.CreateDelegate(method, this).Delegate.Invoke(42);

            Assert.Equal(1, _invocations[methodName]);
        }

        private delegate object TestDelegate_Object_Int32_Int32(int param1, int param2);
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable CA1859 // Use concrete types when possible for improved performance
        private object TestDelegate_Object_Int32_Int32__InstanceFunction_Object_Int32_Int32(int param1, int param2)
        {
            return IncrementInvocationCount(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Object_Int32_Int32));
        }

        private int TestDelegate_Object_Int32_Int32__InstanceFunction_Int32_Int32_Int32(int param1, int param2)
        {
            return IncrementInvocationCount(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Int32_Int32_Int32));
        }

        private object TestDelegate_Object_Int32_Int32__InstanceFunction_Object_Object_Int32(object param1, int param2)
        {
            return IncrementInvocationCount(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Object_Object_Int32));
        }

        private object TestDelegate_Object_Int32_Int32__InstanceFunction_Object_String_Int32(string param1, int param2)
        {
            return IncrementInvocationCount(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Object_String_Int32));
        }
#pragma warning restore CA1859 // Use concrete types when possible for improved performance
#pragma warning restore IDE0060 // Remove unused parameter

        [Theory]
        [InlineData(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Object_Int32_Int32), BindingFlags.Instance | BindingFlags.NonPublic, DelegatorIsCompatibleResultEnum.Compatible)]
        [InlineData(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Int32_Int32_Int32), BindingFlags.Instance | BindingFlags.NonPublic, DelegatorIsCompatibleResultEnum.CompatibleWithCasting)]
        [InlineData(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Object_Object_Int32), BindingFlags.Instance | BindingFlags.NonPublic, DelegatorIsCompatibleResultEnum.CompatibleWithCasting)]
        [InlineData(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Object_String_Int32), BindingFlags.Instance | BindingFlags.NonPublic, DelegatorIsCompatibleResultEnum.Incompatible)]
        public void TestDelegate_Object_Int32_Int32__IsCompatible(string methodName, BindingFlags bindingFlags, DelegatorIsCompatibleResultEnum expected)
        {
            MethodInfo method = typeof(Delegator_Tests).GetMethod(methodName, bindingFlags)!;
            _ = Delegator<TestDelegate_Object_Int32_Int32>.IsCompatible(method, out var result);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Object_Int32_Int32), BindingFlags.Instance | BindingFlags.NonPublic)]
        [InlineData(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Int32_Int32_Int32), BindingFlags.Instance | BindingFlags.NonPublic)]
        [InlineData(nameof(TestDelegate_Object_Int32_Int32__InstanceFunction_Object_Object_Int32), BindingFlags.Instance | BindingFlags.NonPublic)]
        public void TestDelegate_Object_Int32_Int32__Invocation(string methodName, BindingFlags bindingFlags)
        {
            MethodInfo method = typeof(Delegator_Tests).GetMethod(methodName, bindingFlags)!;

            _invocations.Clear();

            Delegator<TestDelegate_Object_Int32_Int32>.CreateDelegate(method, this).Delegate.Invoke(42, 69);

            Assert.Equal(1, _invocations[methodName]);
        }

        private delegate void TestDelegate_Void_Int32(int param);
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable CA1859 // Use concrete types when possible for improved performance
        private static void TestDelegate_Void_Int32__StaticFunction_Void_Int32(int param)
        {
            IncrementInvocationCount(nameof(TestDelegate_Void_Int32__StaticFunction_Void_Int32));
        }

        private void TestDelegate_Void_Int32__InstanceFunction_Void_Int32(int param)
        {
            IncrementInvocationCount(nameof(TestDelegate_Void_Int32__InstanceFunction_Void_Int32));
        }

        private void TestDelegate_Void_Int32__InstanceFunction_Void_Object(object param)
        {
            IncrementInvocationCount(nameof(TestDelegate_Void_Int32__InstanceFunction_Void_Object));
        }

        private void TestDelegate_Void_Int32__InstanceFunction_Void_String(string param)
        {
            IncrementInvocationCount(nameof(TestDelegate_Void_Int32__InstanceFunction_Void_String));
        }
#pragma warning restore CA1859 // Use concrete types when possible for improved performance
#pragma warning restore IDE0060 // Remove unused parameter

        [Theory]
        [InlineData(nameof(TestDelegate_Void_Int32__StaticFunction_Void_Int32), BindingFlags.Static | BindingFlags.NonPublic, DelegatorIsCompatibleResultEnum.Compatible)]
        [InlineData(nameof(TestDelegate_Void_Int32__InstanceFunction_Void_Int32), BindingFlags.Instance | BindingFlags.NonPublic, DelegatorIsCompatibleResultEnum.Compatible)]
        [InlineData(nameof(TestDelegate_Void_Int32__InstanceFunction_Void_Object), BindingFlags.Instance | BindingFlags.NonPublic, DelegatorIsCompatibleResultEnum.CompatibleWithCasting)]
        [InlineData(nameof(TestDelegate_Void_Int32__InstanceFunction_Void_String), BindingFlags.Instance | BindingFlags.NonPublic, DelegatorIsCompatibleResultEnum.Incompatible)]
        public void TestDelegate_Void_Int32__IsCompatible(string methodName, BindingFlags bindingFlags, DelegatorIsCompatibleResultEnum expected)
        {
            MethodInfo method = typeof(Delegator_Tests).GetMethod(methodName, bindingFlags)!;
            _ = Delegator<TestDelegate_Void_Int32>.IsCompatible(method, out var result);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(nameof(TestDelegate_Void_Int32__StaticFunction_Void_Int32), BindingFlags.Static | BindingFlags.NonPublic)]
        [InlineData(nameof(TestDelegate_Void_Int32__InstanceFunction_Void_Int32), BindingFlags.Instance | BindingFlags.NonPublic)]
        [InlineData(nameof(TestDelegate_Void_Int32__InstanceFunction_Void_Object), BindingFlags.Instance | BindingFlags.NonPublic)]
        public void TestDelegate_Void_Int32__Invocation(string methodName, BindingFlags bindingFlags)
        {
            MethodInfo method = typeof(Delegator_Tests).GetMethod(methodName, bindingFlags)!;

            _invocations.Clear();

            Delegator<TestDelegate_Void_Int32>.CreateDelegate(method, this).Delegate.Invoke(42);

            Assert.Equal(1, _invocations[methodName]);
        }

        private static readonly Dictionary<string, int> _invocations = [];
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
            if (_invocations.TryGetValue(method, out int value))
            {
                return value;
            }

            return 0;
        }

    }
}