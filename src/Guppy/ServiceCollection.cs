using Guppy.Attributes;
using Guppy.Enums;
using Guppy.Interfaces;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public sealed class ServiceCollection : List<ServiceDescriptor>
    {
        #region internal Fields
        internal Dictionary<Type, Object> genericSingletons;
        internal Dictionary<Type, IService> typedSingletons;
        internal Dictionary<Type, Func<ServiceProvider, Type, Object>> factories;
        #endregion

        #region Constructors 
        public ServiceCollection()
        {
            this.typedSingletons = new Dictionary<Type, IService>();
            this.genericSingletons = new Dictionary<Type, Object>();
            this.factories = new Dictionary<Type, Func<ServiceProvider, Type, Object>>();
        }
        #endregion

        #region Helper Methods
        internal ServiceProvider BuildServiceProvider()
        {
            return new ServiceProvider(this);
        }

        public void ConfigureMonoGame(GameWindow window, GraphicsDeviceManager graphics)
        {
            this.AddSingleton<GameWindow>(window);
            this.AddSingleton<GraphicsDeviceManager>(graphics);
            this.AddSingleton<GraphicsDevice>(graphics.GraphicsDevice);
            this.AddSingleton<SpriteBatch>(new SpriteBatch(graphics.GraphicsDevice));
        }
        #endregion

        #region Add Factory Methods 
        public void AddFactory(Type type, Func<ServiceProvider, Type, Object> factory)
        {
            this.factories.Add(type, factory);
        }

        public void AddFactory<T>(Func<ServiceProvider, Type, IService> factory)
            where T : IService
        {
            this.AddFactory(typeof(T), factory);
        }
        #endregion

        #region Add Service Methods
        /// <summary>
        /// Add non IService singleton instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public void AddSingleton<T>(T instance)
        {
            this.AddSingleton(typeof(T), instance);
        }
        /// <summary>
        /// Add non IService singleton instance
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        public void AddSingleton(Type type, Object instance)
        {
            ExceptionHelper.ValidateAssignableFrom(type, instance.GetType());

            this.genericSingletons.Add(type, instance);
        }
        public void AddSingleton(String handle, Type type, Action<ServiceProvider, IService> setup = null)
        {
            this.Add(new ServiceDescriptor(handle, Lifetime.Singleton, type, setup));
        }
        public void AddSingleton<T>(String handle, Action<ServiceProvider, T> setup = null)
            where T : IService
        {
            if(setup == null)
                this.AddSingleton(handle, typeof(T));
            else
                this.AddSingleton(handle, typeof(T), (p, i) => setup?.Invoke(p, (T)i));
        }

        public void AddTypedSingleton(String handle, Type type, Type baseType, Action<ServiceProvider, IService> setup = null)
        {
            this.Add(new ServiceDescriptor(handle, Lifetime.TypedSingleton, type, baseType, setup));
        }
        public void AddTypedSingleton<T, TBase>(String handle, Action<ServiceProvider, T> setup = null)
            where T : TBase
            where TBase : IService
        {
            if (setup == null)
                this.AddTypedSingleton(handle, typeof(T), typeof(TBase));
            else
                this.AddTypedSingleton(handle, typeof(T), typeof(TBase), (p, i) => setup?.Invoke(p, (T)i));
        }
        public void AddTypedSingleton<T>(String handle, Action<ServiceProvider, T> setup = null)
            where T : IService
        {
            if (setup == null)
                this.AddTypedSingleton(handle, typeof(T), typeof(T));
            else
                this.AddTypedSingleton(handle, typeof(T), typeof(T), (p, i) => setup?.Invoke(p, (T)i));
        }
        public void AddTypedSingleton<T, TBase>(String handle, T instance)
            where T : TBase
            where TBase : IService
        {
            this.AddTypedSingleton(handle, typeof(T), typeof(TBase));
            this.typedSingletons.Add(typeof(TBase), instance);
        }
        public void AddTypedSingleton<T>(String handle, T instance)
            where T : IService
        {
            this.AddTypedSingleton<T, T>(handle, instance);
        }

        public void AddScoped(String handle, Type type, Action<ServiceProvider, IService> setup = null)
        {
            this.Add(new ServiceDescriptor(handle, Lifetime.Scoped, type, setup));
        }
        public void AddScoped<T>(String handle, Action<ServiceProvider, T> setup = null)
            where T : IService
        {
            if (setup == null)
                this.AddScoped(handle, typeof(T));
            else
                this.AddScoped(handle, typeof(T), (p, i) => setup?.Invoke(p, (T)i));
        }

        public void AddTypedScoped(String handle, Type type, Type baseType, Action<ServiceProvider, IService> setup = null)
        {
            this.Add(new ServiceDescriptor(handle, Lifetime.TypedScoped, type, baseType, setup));
        }
        public void AddTypedScoped<T, TBase>(String handle, Action<ServiceProvider, T> setup = null)
            where T : class, TBase
            where TBase : IService
        {
            if (setup == null)
                this.AddTypedScoped(handle, typeof(T), typeof(TBase));
            else
                this.AddTypedScoped(handle, typeof(T), typeof(TBase), (p, i) => setup?.Invoke(p, (T)i));
        }

        public void AddTransient(String handle, Type type, Action<ServiceProvider, IService> setup = null)
        {
            this.Add(new ServiceDescriptor(handle, Lifetime.Transient, type, setup));
        }
        public void AddTransient<T>(String handle, Action<ServiceProvider, T> setup = null)
            where T : IService
        {
            if (setup == null)
                this.AddTransient(handle, typeof(T));
            else
                this.AddTransient(handle, typeof(T), (p, i) => setup?.Invoke(p, (T)i));
        }

        public void AddSingleton(Type type, Action<ServiceProvider, IService> setup = null)
        {
            this.AddSingleton(type.FullName, type, setup);
        }
        public void AddSingleton<T>(Action<ServiceProvider, T> setup = null)
            where T : IService
        {
            this.AddSingleton<T>(typeof(T).FullName, setup);
        }

        public void AddTypedSingleton(Type type, Type baseType, Action<ServiceProvider, IService> setup = null)
        {
            this.AddTypedSingleton(type.FullName, type, baseType, setup);
        }
        public void AddTypedSingleton<T, TBase>(Action<ServiceProvider, T> setup = null)
            where T : TBase
            where TBase : IService
        {
            this.AddTypedSingleton<T, TBase>(typeof(T).FullName, setup);
        }
        public void AddTypedSingleton<T>(Action<ServiceProvider, T> setup = null)
            where T : IService
        {
            this.AddTypedSingleton<T>(typeof(T).FullName, setup);
        }
        public void AddTypedSingleton<T, TBase>(T instance)
            where T : TBase
            where TBase : IService
        {
            this.AddTypedSingleton<T, TBase>(typeof(T).FullName, instance);
        }
        public void AddTypedSingleton<T>(T instance)
            where T : IService
        {
            this.AddTypedSingleton<T>(typeof(T).FullName, instance);
        }

        public void AddScoped(Type type, Action<ServiceProvider, IService> setup = null)
        {
            this.AddScoped(type.FullName, type, setup);
        }
        public void AddScoped<T>(Action<ServiceProvider, T> setup = null)
            where T : IService
        {
            this.AddScoped<T>(typeof(T).FullName, setup);
        }

        public void AddTypedScoped(Type type, Type baseType, Action<ServiceProvider, IService> setup = null)
        {
            this.AddTypedScoped(type.FullName, type, baseType, setup);
        }
        public void AddTypedScoped<T, TBase>(Action<ServiceProvider, T> setup = null)
            where T : class, TBase
            where TBase : IService
        {
            this.AddTypedScoped<T, TBase>(typeof(T).FullName, setup);
        }

        public void AddTransient(Type type, Action<ServiceProvider, IService> setup = null)
        {
            this.AddTransient(type.FullName, type, setup);
        }
        public void AddTransient<T>(Action<ServiceProvider, T> setup = null)
            where T : IService
        {
            this.AddTransient(typeof(T).FullName, setup);
        }
        #endregion
    }
}
