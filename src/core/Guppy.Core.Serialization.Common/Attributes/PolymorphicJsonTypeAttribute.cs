using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Resources.Serialization.Json;

namespace Guppy.Core.Serialization.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
    public class PolymorphicJsonTypeAttribute(string key, Type? instanceType, Type baseType) : GuppyConfigurationAttribute
    {
        public readonly string Key = key;
        public readonly Type BaseType = baseType;
        public readonly Type? InstanceType = instanceType;

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

    public sealed class PolymorphicJsonTypeAttribute<T>(string key, Type? instanceType = null) : PolymorphicJsonTypeAttribute(key, instanceType, typeof(T))
    {
    }
}
