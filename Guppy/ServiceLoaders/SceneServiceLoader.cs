using Guppy.Attributes;
using Guppy.Interfaces;
using Guppy.Utilities;
using Guppy.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Guppy.Utilities.Pools;

namespace Guppy.ServiceLoaders
{
    public class SceneServiceLoader : IServiceLoader
    {
        public void Boot(IServiceCollection services)
        {
            foreach (Type sceneType in AssemblyHelper.GetTypesWithAttribute<IsSceneAttribute>())
            { // Iterate through all types that contain a scene attribute
                if (!typeof(Scene).IsAssignableFrom(sceneType))
                    throw new Exception($"Invalid type with Scene attribute! {sceneType.Name}");

                // Register the scene type
                // https://stackoverflow.com/questions/987755/get-methodinfo-for-extension-method
                // Call a generic method via recieved type objects. Its dirty and slow, but only runs in the beginning so its okay
                var method = typeof(Guppy.Extensions.DependencyInjection.IServiceCollectionExtenions)
                    .GetMethod("AddScene")
                    .MakeGenericMethod(sceneType);

                // Call the generic addScene method now
                method.Invoke(services, new Object[] { services });
            }
        }

        public void PreInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void Initialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void PostInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
