using System.Text.Json.Serialization;
using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Assets.Serialization.Json;

namespace Guppy.Core.Serialization.Common.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterPolymorphicJsonType(this IGuppyRootBuilder builder, string key, Type instanceType, Type baseType)
        {
            ThrowIf.Type.IsNotAssignableFrom(baseType, instanceType);

            builder.RegisterInstance<PolymorphicJsonType>(new PolymorphicJsonType(key, instanceType, baseType)).SingleInstance();

            return builder;
        }

        public static IGuppyRootBuilder RegisterPolymorphicJsonType<TInstance, TBase>(this IGuppyRootBuilder builer, string key)
            where TInstance : TBase
        {
            return builer.RegisterPolymorphicJsonType(key, typeof(TInstance), typeof(TBase));
        }

        public static IGuppyRootBuilder RegisterJsonConverter<TConverter>(this IGuppyRootBuilder builder)
            where TConverter : JsonConverter
        {
            builder.RegisterType<TConverter>().As<JsonConverter>().SingleInstance();

            return builder;
        }
    }
}