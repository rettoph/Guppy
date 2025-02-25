﻿using System.Reflection;
using System.Reflection.Emit;
using Guppy.Core.Common.Enums;

namespace Guppy.Core.Common.Utilities
{
    public readonly struct Delegator<TDelegate>(TDelegate @delegate, MethodInfo method, object? target)
        where TDelegate : Delegate
    {
        public readonly TDelegate Delegate = @delegate;
        public readonly MethodInfo Method = method;
        public readonly object? Target = target;

        public Delegator(TDelegate @delegate) : this(@delegate, @delegate.GetMethodInfo(), @delegate.Target)
        {
        }

        public override readonly bool Equals(object? obj)
        {
            return obj is Delegator<TDelegate> delegator &&
                   EqualityComparer<MethodInfo>.Default.Equals(this.Method, delegator.Method) &&
                   EqualityComparer<object?>.Default.Equals(this.Target, delegator.Target);
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(this.Method, this.Target);
        }

        public static bool IsCompatible(Type? delegateType, MethodInfo method, out DelegatorIsCompatibleResultEnum result)
        {
            delegateType ??= typeof(TDelegate);

            MethodInfo delegateInvokeMethod = Delegator<TDelegate>.GetInvokeMethod(delegateType);

            bool requiresCasting;
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
                result = DelegatorIsCompatibleResultEnum.Incompatible;
                return false;
            }

            ParameterInfo[] delegateInvokeParameters = delegateInvokeMethod.GetParameters();
            ParameterInfo[] methodParameters = method.GetParameters();
            if (delegateInvokeParameters.Length != methodParameters.Length)
            {
                result = DelegatorIsCompatibleResultEnum.Incompatible;
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

                result = DelegatorIsCompatibleResultEnum.Incompatible;
                return false;
            }

            result = requiresCasting ? DelegatorIsCompatibleResultEnum.CompatibleWithCasting : DelegatorIsCompatibleResultEnum.Compatible;
            return true;
        }

        public static bool IsCompatible(Type? delegateType, MethodInfo method)
        {
            return Delegator<TDelegate>.IsCompatible(delegateType, method, out _);
        }

        public static bool IsCompatible(MethodInfo method, out DelegatorIsCompatibleResultEnum result)
        {
            return Delegator<TDelegate>.IsCompatible(typeof(TDelegate), method, out result);
        }

        public static bool IsCompatible(MethodInfo method)
        {
            return Delegator<TDelegate>.IsCompatible(method, out _);
        }

        public static Delegator<TDelegate> CreateDelegate(Type? delegateType, MethodInfo method, object? target)
        {
            delegateType ??= typeof(TDelegate);

            if (Delegator<TDelegate>.IsCompatible(delegateType, method, out DelegatorIsCompatibleResultEnum result) == false)
            {
                throw new InvalidOperationException();
            }

            if (result == DelegatorIsCompatibleResultEnum.Compatible)
            {
                return new Delegator<TDelegate>(
                    @delegate: (TDelegate)method.CreateDelegate(delegateType, method.IsStatic ? null : target),
                    method: method,
                    target: target
                );
            }

            MethodInfo delegateInvokeMethod = GetInvokeMethod(delegateType);
            ParameterInfo[] delegateInvokeParameters = delegateInvokeMethod.GetParameters();
            ParameterInfo[] methodParameters = method.GetParameters();

            List<Type> dynamicMethodParameterTypes = delegateInvokeParameters.Select(x => x.ParameterType).ToList();
            if (target is not null && method.IsStatic == false)
            {
                dynamicMethodParameterTypes.Insert(0, target.GetType());
            }

            DynamicMethod dynamicMethod = new(
                $"DelegateConverter_Dynamic_{method.Name}",
                delegateInvokeMethod.ReturnType,
                [.. dynamicMethodParameterTypes],
                typeof(Delegator<TDelegate>).Module);


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
                if (WillBoxingOccurOnCast(delegateInvokeParameter.ParameterType, methodParameter.ParameterType))
                {
                    ilGenerator.Emit(OpCodes.Box, delegateInvokeParameter.ParameterType);
                }

                Ldarg_index++;
            }

            ilGenerator.Emit(OpCodes.Call, method);
            ilGenerator.Emit(OpCodes.Ret);

            return new Delegator<TDelegate>(
                @delegate: (TDelegate)dynamicMethod.CreateDelegate(delegateType, method.IsStatic ? null : target),
                method: method,
                target: target
            );
        }

        public static Delegator<TDelegate> CreateDelegate(MethodInfo method, object? target)
        {
            return Delegator<TDelegate>.CreateDelegate(typeof(TDelegate), method, target);
        }

        private static MethodInfo GetInvokeMethod(Type delegateType)
        {
            ThrowIf.Type.IsNotAssignableFrom<Delegate>(delegateType);

            MethodInfo invokeMethod = delegateType.GetMethod("Invoke") ?? throw new NotImplementedException();
            return invokeMethod;
        }

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

        public static bool operator ==(Delegator<TDelegate> left, Delegator<TDelegate> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Delegator<TDelegate> left, Delegator<TDelegate> right)
        {
            return !(left == right);
        }
    }
}