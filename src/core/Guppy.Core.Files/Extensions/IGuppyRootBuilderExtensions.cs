using Autofac;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Files.Serialization.Json;
using Guppy.Core.Files.Services;
using Guppy.Core.Serialization.Common.Extensions;
using Guppy.Core.Serialization.Extensions;

namespace Guppy.Core.Files.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterCoreFileServices(this IGuppyRootBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCoreFileServices), builder =>
            {
                builder.RegisterCoreSerializationServices();

                builder.RegisterJsonConverter<DirectoryPathJsonConverter>();
                builder.RegisterJsonConverter<FilePathJsonConverter>();
                builder.RegisterType<PathService>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<FileService>().AsImplementedInterfaces().SingleInstance();
            });
        }
    }
}