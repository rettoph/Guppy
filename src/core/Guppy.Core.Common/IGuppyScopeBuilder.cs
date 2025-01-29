using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Features.OpenGenerics;

namespace Guppy.Core.Common
{
    public interface IGuppyScopeBuilder
    {
        IGuppyScope? ParentScope { get; }
        ContainerBuilder ContainerBuilder { get; }

        Dictionary<Type, IEnvironmentVariable> EnvironmentVariables { get; }

        IGuppyScope Build();


        IGuppyScopeBuilder SetEnvironmentVariable(IEnvironmentVariable variable)
        {
            this.EnvironmentVariables[variable.GetType()] = variable;

            return this;
        }
        IGuppyScopeBuilder SetEnvironmentVariable<TKey, TValue>(TValue value)
            where TKey : IEnvironmentVariable<TKey, TValue>
        {
            return this.SetEnvironmentVariable(TKey.Create(value));
        }

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
