using Autofac;
using Autofac.Extras.Moq;
using Guppy.Core;
using Guppy.Core.Common;
using Guppy.Core.Common.Enums;
using Guppy.Core.Extensions;
using Guppy.Core.Services;

namespace Guppy.Tests.Common
{
    public class GuppyScopeMocker
    {
        private Action<IGuppyScopeBuilder>? _builders;
        private Action<AutoMock>? _mockers;
        private IGuppyScope? _scope;
        private readonly EnvironmentVariableService _environmentVariableService;
        private readonly GuppyScopeTypeEnum _type;

        protected IGuppyScope scope => this._scope ??= this.BuildGuppyScope();

        public GuppyScopeMocker(GuppyScopeTypeEnum type = GuppyScopeTypeEnum.Child, IEnumerable<IEnvironmentVariable>? environment = null)
        {
            this._environmentVariableService = new EnvironmentVariableService(environment ?? []);
            this._type = type;
            this.Register(x => x.RegisterCoreServices(this._environmentVariableService));
        }

        protected internal void Register(Action<IGuppyScopeBuilder> builder)
        {
            if (this._scope is not null)
            {
                throw new InvalidOperationException();
            }

            this._builders += builder;
        }

        protected internal void Mock(Action<AutoMock> mocker)
        {
            if (this._scope is not null)
            {
                throw new InvalidOperationException();
            }

            this._mockers += mocker;
        }

        private IGuppyScope BuildGuppyScope()
        {
            AutoMock mock = AutoMock.GetLoose(containerBuilder =>
            {
                GuppyScopeBuilder guppyScopeBuilder = new(this._type, this._environmentVariableService, null, containerBuilder);
                this._builders?.Invoke(guppyScopeBuilder);
            });

            this._mockers?.Invoke(mock);

            return mock.Container.Resolve<IGuppyScope>();
        }
    }

    public class GuppyScopeMocker<TSelf, TService>(GuppyScopeTypeEnum type = GuppyScopeTypeEnum.Child, IEnumerable<IEnvironmentVariable>? environment = null) : GuppyScopeMocker(type, environment)
        where TSelf : GuppyScopeMocker<TSelf, TService>
        where TService : notnull
    {
        public virtual TService Build()
        {
            return this.scope.Resolve<TService>();
        }

        public new TSelf Register(Action<IGuppyScopeBuilder> builder)
        {
            base.Register(builder);

            return (TSelf)this;
        }

        public new TSelf Mock(Action<AutoMock> mocker)
        {
            base.Mock(mocker);

            return (TSelf)this;
        }
    }
}