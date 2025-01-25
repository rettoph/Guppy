using Guppy.Core.Common;

namespace Guppy.Core.Commands.Common.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterCommand<T>(this IGuppyScopeBuilder builder)
            where T : ICommand
        {
            builder.RegisterType<T>().As<ICommand>().SingleInstance();

            return builder;
        }
    }
}