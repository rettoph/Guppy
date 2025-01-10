using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Files.Serialization.Json;
using Guppy.Core.Files.Services;
using Guppy.Core.Serialization.Common.Extensions;
using Guppy.Core.Serialization.Extensions;

namespace Guppy.Core.Files.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCoreFileServices(this ContainerBuilder builder)
        {
            if (builder.HasTag(nameof(RegisterCoreFileServices)))
            {
                return builder;
            }

            builder.RegisterCoreSerializationServices();

            builder.RegisterJsonConverter<DirectoryLocationJsonConverter>();
            builder.RegisterJsonConverter<FileLocationJsonConverter>();
            builder.RegisterType<PathService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<FileService>().AsImplementedInterfaces().SingleInstance();

            return builder.AddTag(nameof(RegisterCoreFileServices));
        }
    }
}