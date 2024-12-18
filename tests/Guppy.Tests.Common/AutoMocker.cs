﻿using Autofac;
using Autofac.Extras.Moq;
using Guppy.Core.Common.Contexts;
using Guppy.Core.Extensions;
using Moq;

namespace Guppy.Tests.Common
{
    public abstract class AutoMocker
    {
        private Action<ContainerBuilder>? _builders;
        private Action<AutoMock>? _mockers;
        private AutoMock? _automock;

        protected AutoMock AutoMock => _automock ??= this.BuildAutoMock();

        internal AutoMocker()
        {
            this.Register(x => x.RegisterCoreServices(
                context: new Mock<IGuppyContext>().Object));
        }

        protected internal void Register(Action<ContainerBuilder> builder)
        {
            if (_automock is not null)
            {
                throw new InvalidOperationException();
            }

            _builders += builder;
        }

        protected internal void Mock(Action<AutoMock> mocker)
        {
            if (_automock is not null)
            {
                throw new InvalidOperationException();
            }

            _mockers += mocker;
        }

        private AutoMock BuildAutoMock()
        {
            AutoMock mock = AutoMock.GetLoose(builder => _builders?.Invoke(builder));
            _mockers?.Invoke(mock);

            return mock;
        }
    }

    public class AutoMocker<TSelf, TService>() : AutoMocker()
        where TSelf : AutoMocker<TSelf, TService>
        where TService : notnull
    {
        public virtual TService Build()
        {
            return this.AutoMock.Container.Resolve<TService>();
        }

        public new TSelf Register(Action<ContainerBuilder> builder)
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
