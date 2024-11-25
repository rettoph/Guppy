using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Resources.Serialization.Json;

namespace Guppy.Core.Serialization.Common.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterPolymorphicJsonType(this ContainerBuilder builder, string key, Type instanceType, Type baseType)
        {
            ThrowIf.Type.IsNotAssignableFrom(baseType, instanceType);

            builder.RegisterInstance<PolymorphicJsonType>(new PolymorphicJsonType(key, instanceType, baseType)).SingleInstance();

            return builder;
        }

        public static ContainerBuilder RegisterPolymorphicJsonType<TInstance, TBase>(this ContainerBuilder builer, string key)
            where TInstance : TBase
        {
            return builer.RegisterPolymorphicJsonType(key, typeof(TInstance), typeof(TBase));
        }
    }
}
