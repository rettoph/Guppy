using Guppy.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Pools
{
    /// <summary>
    /// Pool implementation that requires the input target type
    /// to be assignable to Creatable
    /// </summary>
    public class CreatablePool : Pool
    {
        public CreatablePool(Type targetType) : base(targetType)
        {
            ExceptionHelper.ValidateAssignableFrom<Creatable>(targetType);
        }

        protected override Object Build(IServiceProvider provider)
        {
            var instance = ActivatorUtilities.CreateInstance(provider, this.TargetType) as Creatable;

            instance.TryCreate(provider);

            return instance;
        }

        public override T Pull<T>(Action<T> setup = null)
        {
            var instance =  base.Pull(setup);


            (instance as Creatable).Events.Add<Creatable>("disposing", this.HandleInstanceDisposing);

            return instance;
        }

        private void HandleInstanceDisposing(object sender, Creatable instance)
        {
            this.logger.LogTrace($"Pool<{this.GetType().Name}>({this.Id}) => Putting Type<{instance.GetType().Name}>({instance.Id}) instance back into pool.");
            this.Put(instance);
        }
    }
}
