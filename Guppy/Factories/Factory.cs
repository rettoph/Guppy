using Guppy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Factories
{
    public abstract class Factory<T>
        where T : IUniqueObject
    {
        protected Type targetType;

        protected Factory()
        {
            this.targetType = typeof(T);
        }

        public virtual T Create(IServiceProvider provider)
        {
            return (T)this.Create(provider, typeof(T));
        }
        public virtual T Create(IServiceProvider provider, params Object[] args)
        {
            return (T)this.Create(provider, typeof(T), args);
        }

        // Create a new instance of the current factory object
        public virtual TInstance Create<TInstance>(IServiceProvider provider, params Object[] args)
            where TInstance : class, T
        {
            return this.Create(provider, typeof(TInstance), args) as TInstance;
        }

        protected virtual Object Create(IServiceProvider provider, Type type, params Object[] args)
        {
            if (!this.targetType.IsAssignableFrom(type))
                throw new Exception("Unable to create type instance, factory type not assignable from target type!");
            else if (!type.IsClass)
                throw new Exception("Unable to create type instance, target is not a class!");
            else if (type.IsAbstract)
                throw new Exception("Unable to create type instance, target is abstract!");

            var instance = (T)ActivatorUtilities.CreateInstance(provider, type, args);

            provider.GetService<ILogger>().LogDebug($"Created new {instance.GetType().Name}({instance.Id}).");

            return instance;
        }
    }
}
