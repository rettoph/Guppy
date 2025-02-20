using Autofac;
using Autofac.Extras.Moq;
using Guppy.Core.Builders;
using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Extensions;

namespace Guppy.Tests.Common
{
    public class GuppyRootMocker : IDisposable
    {
        private AutoMock? _mock;
        private Action<IGuppyRootBuilder>? _builders;
        private Action<AutoMock>? _mockers;
        private IGuppyRoot? _root;
        private readonly IEnvironmentVariable[] _environmentVariables;

        protected IGuppyRoot root => this._root ??= this.BuildGuppyRoot();

        public GuppyRootMocker(IEnumerable<IEnvironmentVariable>? environment = null)
        {
            this._environmentVariables = [.. environment ?? []];
            this.Register(x => x.RegisterCoreServices());
        }

        protected internal void Register(Action<IGuppyRootBuilder> build)
        {
            if (this._root is not null)
            {
                throw new InvalidOperationException();
            }

            this._builders += build;
        }

        protected internal void Mock(Action<AutoMock> mocker)
        {
            if (this._root is not null)
            {
                throw new InvalidOperationException();
            }

            this._mockers += mocker;
        }

        private IGuppyRoot BuildGuppyRoot()
        {
            this._mock = AutoMock.GetLoose(containerBuilder =>
            {
                GuppyRootBuilder guppyScopeBuilder = new(this._environmentVariables, containerBuilder);
                this._builders?.Invoke(guppyScopeBuilder);
            });

            this._mockers?.Invoke(this._mock);

            return this._mock.Container.Resolve<IGuppyRoot>();
        }

        public void Dispose()
        {
            this._mock?.Dispose();
        }
    }

    public class GuppyRootMocker<TSelf, TService>(IEnumerable<IEnvironmentVariable>? environment = null) : GuppyRootMocker(environment)
        where TSelf : GuppyRootMocker<TSelf, TService>
        where TService : class
    {
        public virtual TService Build()
        {
            return this.root.Resolve<TService>();
        }

        public new TSelf Register(Action<IGuppyRootBuilder> build)
        {
            base.Register(build);

            return (TSelf)this;
        }

        public new TSelf Mock(Action<AutoMock> mocker)
        {
            base.Mock(mocker);

            return (TSelf)this;
        }
    }

    public class GuppyRootMocker<TService>(IEnumerable<IEnvironmentVariable>? environment = null) : GuppyRootMocker<GuppyRootMocker<TService>, TService>(environment)
        where TService : class
    {

    }
}