using Autofac;
using Guppy.Attributes;
using Guppy.Common;
using Guppy.Resources.Serialization.Json;
using Guppy.Serialization;

namespace Guppy.Resources.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple =true)]
    public class PolymorphicJsonTypeAttribute : GuppyConfigurationAttribute
    {
        public readonly string Key;
        public readonly Type? Type;

        public PolymorphicJsonTypeAttribute(string key, Type? type = null)
        {
            this.Key = key;
            this.Type = type;

            if(this.Type == null)
            {
                return;
            }

            ThrowIf.Type.IsNotGenericType(this.Type);
        }

        protected override void Configure(ContainerBuilder builder, Type classType)
        {
            if(classType.IsGenericTypeDefinition && this.Type?.GetGenericTypeDefinition() != classType)
            {
                return;
            }

            builder.RegisterInstance<PolymorphicJsonType>(new PolymorphicJsonType(this.Key, this.Type ?? classType)).SingleInstance();
        }
    }

    public sealed class PolymorphicJsonTypeAttribute<T> : PolymorphicJsonTypeAttribute
    {
        public PolymorphicJsonTypeAttribute(string key) : base(key, typeof(T))
        {
        }
    }
}
