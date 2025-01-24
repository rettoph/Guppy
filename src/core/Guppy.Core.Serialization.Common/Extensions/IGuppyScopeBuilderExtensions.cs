using System.Text.Json.Serialization;
using Guppy.Core.Common;
using Guppy.Core.Resources.Serialization.Json;

namespace Guppy.Core.Serialization.Common.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterPolymorphicJsonType(this IGuppyScopeBuilder builder, string key, Type instanceType, Type baseType)
        {
            ThrowIf.Type.IsNotAssignableFrom(baseType, instanceType);

            builder.RegisterInstance<PolymorphicJsonType>(new PolymorphicJsonType(key, instanceType, baseType)).SingleInstance();

            return builder;
        }

        public static IGuppyScopeBuilder RegisterPolymorphicJsonType<TInstance, TBase>(this IGuppyScopeBuilder builer, string key)
            where TInstance : TBase
        {
            return builer.RegisterPolymorphicJsonType(key, typeof(TInstance), typeof(TBase));
        }

        public static IGuppyScopeBuilder RegisterJsonConverter<TConverter>(this IGuppyScopeBuilder builder)
            where TConverter : JsonConverter
        {
            builder.RegisterType<TConverter>().As<JsonConverter>().SingleInstance();

            return builder;
        }
    }
}