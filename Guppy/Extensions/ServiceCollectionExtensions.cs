using Guppy.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions
{
    /// <summary>
    /// Add custom game extensions to the main
    /// ServiceCollection class.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static void AddScene<TScene>(this ServiceCollection collection)
            where TScene : Scene
        {
            collection.AddScoped<TScene>(SceneFactory<TScene>.BuildFactory<TScene>().Create);
        }
    }
}
