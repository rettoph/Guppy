using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;

namespace Guppy.Core.Commands.Common.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterCommand<T>(this IGuppyRootBuilder builder)
            where T : ICommand
        {
            builder.RegisterType<T>().As<ICommand>().SingleInstance();

            return builder;
        }
    }
}