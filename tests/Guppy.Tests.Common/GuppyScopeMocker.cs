using Autofac;
using Autofac.Extras.Moq;
using Guppy.Core;
using Guppy.Core.Common;
using Guppy.Core.Common.Enums;
using Guppy.Core.Extensions;

namespace Guppy.Tests.Common
{
    public class GuppyScopeMocker
    {
        private Action<IGuppyScopeBuilder>? _builders;
        private Action<AutoMock>? _mockers;
        private IGuppyScope? _scope;

        protected IGuppyScope scope => this._scope ??= this.BuildGuppyScope();

        internal GuppyScopeMocker()
        {
            this.Register(x => x.RegisterCoreServices());
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
                GuppyScopeBuilder guppyScopeBuilder = new([], GuppyScopeTypeEnum.Child, null, containerBuilder);
                this._builders?.Invoke(guppyScopeBuilder);
            });

            this._mockers?.Invoke(mock);

            return mock.Container.Resolve<IGuppyScope>();
        }
    }

    public class GuppyScopeMocker<TSelf, TService>() : GuppyScopeMocker()
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