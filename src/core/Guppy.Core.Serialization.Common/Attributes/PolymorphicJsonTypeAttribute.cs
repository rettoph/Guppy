using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Resources.Serialization.Json;

namespace Guppy.Core.Serialization.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
    public class PolymorphicJsonTypeAttribute : GuppyConfigurationAttribute
    {
        public readonly string Key;
        public readonly Type BaseType;
        public readonly Type? InstanceType;

        public PolymorphicJsonTypeAttribute(string key, Type? instanceType, Type baseType)
        {
            this.Key = key;
            this.BaseType = baseType;
            this.InstanceType = instanceType;
        }

        protected override void Configure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            Type instanceType = this.InstanceType ?? classType;

            if (instanceType.IsGenericTypeDefinition)
            {
                throw new Exception();
            }

            builder.RegisterInstance<PolymorphicJsonType>(new PolymorphicJsonType(this.Key, instanceType, this.BaseType)).SingleInstance();
        }
    }

    public sealed class PolymorphicJsonTypeAttribute<T> : PolymorphicJsonTypeAttribute
    {
        public PolymorphicJsonTypeAttribute(string key, Type? instanceType = null) : base(key, instanceType, typeof(T))
        {
        }
    }
}
