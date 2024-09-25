using System.Reflection;
using System.Reflection.Emit;

namespace Guppy.Core.Common.Helpers
{
    public static class DelegateHelper
    {
        public enum IsCompatibleResultEnum
        {
            Incompatible,
            Compatible,
            CompatibleWithCasting
        }

        public static bool IsCompatible(Type delegateType, MethodInfo method, out IsCompatibleResultEnum result)
        {
            MethodInfo delegateInvokeMethod = DelegateHelper.GetInvokeMethod(delegateType);

            bool requiresCasting = false;

            if (method.ReturnType == delegateInvokeMethod.ReturnType)
            {
                requiresCasting = false;
            }
            else if (method.ReturnType.IsAssignableTo(delegateInvokeMethod.ReturnType) == true)
            {
                requiresCasting = true;
            }
            else
            {
                result = IsCompatibleResultEnum.Incompatible;
                return false;
            }

            ParameterInfo[] delegateInvokeParameters = delegateInvokeMethod.GetParameters();
            ParameterInfo[] methodParameters = method.GetParameters();
            if (delegateInvokeParameters.Length != methodParameters.Length)
            {
                result = IsCompatibleResultEnum.Incompatible;
                return false;
            }

            for (int i = 0; i < delegateInvokeParameters.Length; i++)
            {
                ParameterInfo delegateInvokeParameter = delegateInvokeParameters[i];
                ParameterInfo methodParameter = methodParameters[i];

                if (delegateInvokeParameter.ParameterType == methodParameter.ParameterType)
                {
                    continue;
                }

                if (delegateInvokeParameter.ParameterType.IsAssignableTo(methodParameter.ParameterType))
                {
                    requiresCasting = true;
                    continue;
                }

                result = IsCompatibleResultEnum.Incompatible;
                return false;
            }

            result = requiresCasting ? IsCompatibleResultEnum.CompatibleWithCasting : IsCompatibleResultEnum.Compatible;
            return true;
        }

        public static bool IsCompatible<TDelegate>(MethodInfo method, out IsCompatibleResultEnum result)
            where TDelegate : Delegate
                => DelegateHelper.IsCompatible(typeof(TDelegate), method, out result);

        public static bool IsCompatible<TDelegate>(MethodInfo method)
            where TDelegate : Delegate
                => DelegateHelper.IsCompatible<TDelegate>(method, out _);

        public static Delegate CreateDelegate(Type delegateType, MethodInfo method, object? target)
        {
            if (DelegateHelper.IsCompatible(delegateType, method, out IsCompatibleResultEnum result) == false)
            {
                throw new InvalidOperationException();
            }

            if (result == IsCompatibleResultEnum.Compatible)
            {
                return method.CreateDelegate(delegateType, method.IsStatic ? null : target);
            }

            MethodInfo delegateInvokeMethod = DelegateHelper.GetInvokeMethod(delegateType);
            ParameterInfo[] delegateInvokeParameters = delegateInvokeMethod.GetParameters();
            ParameterInfo[] methodParameters = method.GetParameters();

            List<Type> dynamicMethodParameterTypes = delegateInvokeParameters.Select(x => x.ParameterType).ToList();
            if (target is not null && method.IsStatic == false)
            {
                dynamicMethodParameterTypes.Insert(0, target.GetType());
            }

            DynamicMethod dynamicMethod = new DynamicMethod(
                $"DelegateConverter_Dynamic_{method.Name}",
                delegateInvokeMethod.ReturnType,
                dynamicMethodParameterTypes.ToArray(),
                typeof(DelegateHelper).Module);


            // Get an ILGenerator to emit the IL code for the method body
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();

            // Load arguments
            int Ldarg_index = 0;
            if (target is not null && method.IsStatic == false)
            {
                ilGenerator.Emit(OpCodes.Ldarg, Ldarg_index);
                Ldarg_index++;
            }

            for (int i = 0; i < delegateInvokeParameters.Length; i++)
            {
                ParameterInfo delegateInvokeParameter = delegateInvokeParameters[i];
                ParameterInfo methodParameter = methodParameters[i];

                dynamicMethod.DefineParameter(Ldarg_index, ParameterAttributes.In, methodParameter.Name);

                ilGenerator.Emit(OpCodes.Ldarg, Ldarg_index);
                if (DelegateHelper.WillBoxingOccurOnCast(delegateInvokeParameter.ParameterType, methodParameter.ParameterType))
                {
                    ilGenerator.Emit(OpCodes.Box, delegateInvokeParameter.ParameterType);
                }

                Ldarg_index++;
            }

            ilGenerator.Emit(OpCodes.Call, method);
            ilGenerator.Emit(OpCodes.Ret);

            Delegate del = dynamicMethod.CreateDelegate(delegateType, method.IsStatic ? null : target);
            return del;
        }

        public static TDelegate CreateDelegate<TDelegate>(MethodInfo method, object? target)
            where TDelegate : Delegate
                => (TDelegate)DelegateHelper.CreateDelegate(typeof(TDelegate), method, target);

        private static MethodInfo GetInvokeMethod(Type delegateType)
        {
            ThrowIf.Type.IsNotAssignableFrom<Delegate>(delegateType);

            MethodInfo invokeMethod = delegateType.GetMethod("Invoke") ?? throw new NotImplementedException();
            return invokeMethod;
        }

        private static MethodInfo GetInvokeMethod<TDelegate>()
            where TDelegate : Delegate
                => DelegateHelper.GetInvokeMethod(typeof(TDelegate));

        private static bool WillBoxingOccurOnCast(Type sourceType, Type targetType)
        {
            if (sourceType == targetType)
            {
                return false;
            }

            if (sourceType.IsValueType == targetType.IsValueType)
            {
                return false;
            }

            return true;
        }
    }
}
