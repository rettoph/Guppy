using Guppy.Configurations;
using Guppy.Factories;
using Guppy.Implementations;
using Guppy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static void AddGame<TGame>(this IServiceCollection collection)
            where TGame : Game
        {
            collection.AddScoped<TGame>(
                p => p.GetRequiredService<GameFactory>().Create<TGame>(p));
        }

        public static void AddScene<TScene>(this IServiceCollection collection)
            where TScene : Scene
        {
            collection.AddScoped<TScene>(
                p => p.GetRequiredService<SceneFactory>().Create<TScene>(p));
        }

        public static void AddLayer<TLayer>(this IServiceCollection collection)
            where TLayer : Layer
        {
            var factory = LayerFactory<TLayer>.BuildFactory<TLayer>();
            collection.AddSingleton<LayerFactory<TLayer>>(factory); // Add the new factory (for future custom creation reference)
            collection.AddTransient<TLayer>(factory.Create); // Add the factory's create method as the default constructor
        }

        public static void AddLoader<TLoader>(this IServiceCollection collection)
            where TLoader : class, ILoader
        {
            // Add the loader as a singleton
            
            // Create a factory method for direct reference in dependency injection
            var factory = LoaderFactory<TLoader>.BuildFactory<TLoader>();
            collection.AddSingleton(factory);
            collection.AddSingleton<TLoader>(factory.Create);
            collection.AddSingleton<ILoader, TLoader>(p => p.GetRequiredService<TLoader>());
        }

        public static void AddDriver<TDriven, TDriver>(this IServiceCollection collection, UInt16 priority = 100)
            where TDriven : Driven
            where TDriver : Driver
        {
            collection.AddSingleton(
                new DriverConfiguration(typeof(TDriven), typeof(TDriver), priority));
        }
    }
}
