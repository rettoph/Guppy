using Guppy.Collections;
using Guppy.Utilities.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Pools
{
    /// <summary>
    /// Special pool used to create scoped scene instances.
    /// </summary>
    internal class ScenePool : InitializablePool
    {
        public ScenePool(Type targetType) : base(targetType)
        {
            if (!typeof(Scene).IsAssignableFrom(targetType))
                throw new Exception($"Unable to create ScenePool. TargetType must be assignable to Scene. Input {targetType.Name} is not.");
        }

        protected override object Build(IServiceProvider provider)
        {
            // Create a new scope and use it to create a new scene
            var scopedProvider = provider.CreateScope().ServiceProvider;
            var instance = base.Build(scopedProvider) as Scene;

            // Update the scopes internal scene options...
            scopedProvider.GetService<SceneOptions>().Instance = instance;

            return instance;
        }

        public override T Pull<T>(Action<T> setup = null)
        {
            var scene = base.Pull(setup) as Scene;


            return scene as T;
        }
    }
}
