﻿using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Features.OpenGenerics;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Providers;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Common
{
    public interface IGuppyScopeBuilder : IGuppyVariableProvider<IScopeVariable>
    {
        IEnvironmentVariableService EnvironmentVariables { get; }
        IGuppyScope? ParentScope { get; }
        ContainerBuilder ContainerBuilder { get; }

        IGuppyScopeBuilder AddScopeVariable(IScopeVariable variable);

        IGuppyScopeBuilder AddScopeVariable<TKey, TValue>(TValue value)
            where TKey : IScopeVariable<TKey, TValue>
            where TValue : notnull;

        TVariable? GetScopeVariable<TVariable>()
            where TVariable : IScopeVariable;

        IGuppyScopeBuilder Filter(Action<GuppyScopeFilterBuilder> filterBuilder, Action<IGuppyScopeBuilder> scopeBuilder);

        IGuppyScope Build();

        IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> RegisterInstance<T>(T instance)
            where T : class
        {
            return this.ContainerBuilder.RegisterInstance(instance);
        }

        IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> Register<T>(Func<IComponentContext, T> factory)
            where T : class
        {
            return this.ContainerBuilder.Register(factory);
        }

        IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> Register<T>(Func<ILifetimeScope, T> factory)
            where T : class
        {
            return this.ContainerBuilder.Register(factory);
        }

        IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterType<T>()
            where T : notnull
        {
            return this.ContainerBuilder.RegisterType<T>();
        }

        IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterType(Type implementationType)
        {
            return this.ContainerBuilder.RegisterType(implementationType);
        }

        IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> RegisterGeneric(Type implementor)
        {
            return this.ContainerBuilder.RegisterGeneric(implementor);
        }

        IRegistrationBuilder<object, OpenGenericDelegateActivatorData, DynamicRegistrationStyle> RegisterGeneric(Func<IComponentContext, Type[], object> factory)
        {
            return this.ContainerBuilder.RegisterGeneric(factory);
        }

        IModuleRegistrar RegisterModule<T>()
            where T : IModule, new()
        {
            return this.ContainerBuilder.RegisterModule<T>();
        }
    }
}
