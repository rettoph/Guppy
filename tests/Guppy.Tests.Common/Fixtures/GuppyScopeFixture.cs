using Autofac;
using Autofac.Extras.Moq;
using Guppy.Core;
using Guppy.Core.Common;
using Guppy.Core.Common.Enums;
using Guppy.Core.Extensions;
using Guppy.Core.Logging.Common;
using Guppy.Core.Logging.Common.Services;
using Guppy.Core.Services;
using Guppy.Tests.Common.Extensions;
using Moq;

namespace Guppy.Tests.Common.Fixtures
{
    public class GuppyScopeFixture : IDisposable
    {
        public readonly AutoMock AutoMock;
        public readonly GuppyScope GuppyScope;
        public readonly Mocker<ILoggerService> LoggerServiceMock;

        public GuppyScopeFixture(GuppyScopeTypeEnum type, IEnumerable<IEnvironmentVariable> environment, Action<IGuppyScopeBuilder> build)
        {
            this.AutoMock = AutoMock.GetLoose(containerBuilder =>
            {
                EnvironmentVariableService environmentVariableService = new(environment);
                GuppyScopeBuilder guppyScopeBuilder = new(type, environmentVariableService, null, containerBuilder);

                guppyScopeBuilder.RegisterCoreServices(environmentVariableService);

                build.Invoke(guppyScopeBuilder);
            });

            this.GuppyScope = this.AutoMock.Container.Resolve<GuppyScope>();

            this.LoggerServiceMock = this.AutoMock.Mocker<ILoggerService>()
                .Setup(loggers => loggers.GetLogger(It.IsAny<Type>()), () => new Mocker<ILogger>().GetInstance())
                .Setup(loggers => loggers.GetLogger<It.IsAnyType>(), new InvocationFunc(invocation =>
                {
                    Type loggerContext = invocation.Method.ReturnType.GenericTypeArguments[0];
                    return Mocker.GetGenericInstance(typeof(ILogger<>), loggerContext);
                }));
        }

        public virtual void Dispose()
        {
            this.AutoMock.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
