using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Files.Serialization.Json;
using Guppy.Core.Files.Services;
using Guppy.Core.Serialization.Common.Extensions;
using Guppy.Core.Serialization.Extensions;

namespace Guppy.Core.Files.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterCoreFileServices(this IGuppyScopeBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCoreFileServices), builder =>
            {
                builder.RegisterCoreSerializationServices();

                builder.RegisterJsonConverter<DirectoryLocationJsonConverter>();
                builder.RegisterJsonConverter<FileLocationJsonConverter>();
                builder.RegisterType<PathService>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<FileService>().AsImplementedInterfaces().SingleInstance();
            });
        }
    }
}