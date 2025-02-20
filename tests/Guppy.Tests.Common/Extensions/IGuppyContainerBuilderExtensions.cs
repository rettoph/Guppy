using Autofac.Builder;
using Autofac.Extras.Moq;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Moq;

namespace Guppy.Tests.Common.Extensions
{
    public static class IGuppyContainerBuilderExtensions
    {
        /// <summary>
        /// Register a mock by explicitly providing a Mock instance for the service being mocked.
        /// </summary>
        /// <typeparam name="TMocked">The type of service.</typeparam>
        /// <param name="builder">The container builder.</param>
        /// <param name="mocker">The mock.</param>
        /// <returns>The registration builder.</returns>
        public static IRegistrationBuilder<TMocked, SimpleActivatorData, SingleRegistrationStyle> RegisterMocker<TMocked>(this IGuppyContainerBuilder builder, Mocker<TMocked> mocker)
            where TMocked : class
        {
            ArgumentNullException.ThrowIfNull(mocker);

            return builder.RegisterInstance(mocker.AsMock().Object).As<TMocked>().ExternallyOwned();
        }

        /// <summary>
        /// Register a mock by explicitly providing a Mock instance for the service being mocked.
        /// </summary>
        /// <typeparam name="TMocked">The type of service.</typeparam>
        /// <param name="builder">The container builder.</param>
        /// <param name="mocker">The mock.</param>
        /// <returns>The registration builder.</returns>
        public static IRegistrationBuilder<TMocked, SimpleActivatorData, SingleRegistrationStyle> RegisterMock<TMocked>(this IGuppyContainerBuilder builder)
            where TMocked : class
        {
            return builder.ContainerBuilder.RegisterMock(new Mock<TMocked>());
        }
    }
}