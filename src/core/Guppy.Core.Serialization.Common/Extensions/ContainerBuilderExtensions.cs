﻿using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Resources.Serialization.Json;
using System.Text.Json.Serialization;

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
        public static ContainerBuilder RegisterJsonConverter<TConverter>(this ContainerBuilder builder)
            where TConverter : JsonConverter
        {
            builder.RegisterType<TConverter>().As<JsonConverter>().SingleInstance();

            return builder;
        }
    }
}
