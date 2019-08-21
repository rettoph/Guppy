using Guppy.Implementations;
using Microsoft.Extensions.DependencyInjection;
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
            if (!typeof(Creatable).IsAssignableFrom(targetType))
                throw new Exception($"Unable to create CreatablePool. TargetType must be assignable to Creatable. Input {targetType.Name} is not.");
        }

        protected override Object Create(IServiceProvider provider)
        {
            var instance = ActivatorUtilities.CreateInstance(provider, this.TargetType) as Creatable;

            instance.TryCreate(provider);

            return instance;
        }

        public override T Pull<T>(IServiceProvider provider, Action<T> setup = null)
        {
            var instance =  base.Pull(provider, setup);


            (instance as Creatable).Events.Add<Creatable>("disposing", this.HandleInstanceDisposing);

            return instance;
        }

        private void HandleInstanceDisposing(object sender, Creatable instance)
        {
            this.Put(instance);
        }
    }
}
