using Guppy.DependencyInjection.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection
{
    public class TypeFactory : TypeFactoryDescriptor
    {
        #region Public Properties
        /// <summary>
        /// The primary pool for the raw type. This is used by default
        /// unless a dynamic generic type is being built.
        /// </summary>
        public TypePool Pool { get; private set; }

        /// <summary>
        /// A collection of pools creatable by this factory. Generally this
        /// is only used to create the initial pool, but may be used to create
        /// additionals as generic type instances get built.
        /// </summary>
        public TypePoolManager Pools { get; private set; }
        #endregion

        #region Constructors
        public TypeFactory(
            TypeFactoryDescriptor descriptor
        ) : base(descriptor.Type, descriptor.Method, descriptor.MaxPoolSize, descriptor.Priority)
        {
            this.Pools = new TypePoolManager(ref this.MaxPoolSize);
            this.Pool = this.Pools[this.Type];
        }
        #endregion

        #region Helper Methods
        public Object Build(ServiceProvider provider, Type[] generics = default, Action<Object> postBuild = default)
        {
            Type type = this.Type;
            TypePool pool = this.Pool;

            if(generics != default && generics.Any())
            { // Generic type overriding...
                type = type.MakeGenericType(generics);
                pool = this.Pools[type];
            }

            if (pool.Any())
                return pool.Pull();

            var instance = this.Method(provider, type);
            postBuild?.Invoke(instance);

            return instance;
        }
        #endregion
    }
}
