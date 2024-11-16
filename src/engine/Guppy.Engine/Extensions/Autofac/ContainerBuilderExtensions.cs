﻿using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Serilog;

namespace Guppy.Engine.Extensions.Autofac
{
    internal static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterBootServices(this ContainerBuilder builder)
        {
            builder.Configure<LoggerConfiguration>((scope, config) =>
            {
                config.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
            });

            return builder;
        }
    }
}
