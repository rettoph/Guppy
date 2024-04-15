﻿using Autofac;
using Autofac.Features.ResolveAnything;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Contexts;
using Guppy.Core.Common.Services;
using Guppy.Core.Extensions;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Loaders;
using System.Reflection;

namespace Guppy.Engine
{
    public sealed class GuppyEngine : IGuppyEngine, IDisposable
    {
        private readonly IGuppyContext _context;
        private readonly ILifetimeScope _scope;

        IGuppyContext IGuppyEngine.Context => _context;

        public GuppyEngine(IGuppyContext context, ILifetimeScope scope)
        {
            _context = context;
            _scope = scope;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        T IGuppyEngine.Resolve<T>()
        {
            return _scope.Resolve<T>();
        }

        #region Static
        private static readonly object _lock;
        private static IGuppyEngine? _instance;

        public static IGuppyContext? Context => _instance?.Context;

        static GuppyEngine()
        {
            _lock = new object();
        }

        public static IGuppyEngine Start(GuppyContext context, Action<ContainerBuilder>? builder = null)
        {
            lock (_lock)
            {
                if (_instance is not null)
                {
                    throw new Exception($"{nameof(GuppyEngine)}::{nameof(Start)} - An {nameof(IGuppyEngine)} instance has already been created.");
                }

                // Construct a service container to store factory related services
                // Used for boot loggers, engine loaders, and related services
                ContainerBuilder bootBuilder = new ContainerBuilder();
                bootBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
                bootBuilder.RegisterCoreServices(context);

                // Construct the factory service and prepare for assembly loading.
                IContainer boot = bootBuilder.Build();
                IAssemblyService assemblies = boot.Resolve<IAssemblyService>();

                assemblies.OnAssemblyLoaded += HandleAssemblyLoaded;
                assemblies.Load(boot.Resolve<IGuppyContext>().Entry);
                assemblies.OnAssemblyLoaded -= HandleAssemblyLoaded;

                // Begin boot phase 2 - call all boot attributes

                // Construct the engine container
                ContainerBuilder engineBuilder = new ContainerBuilder();
                engineBuilder.RegisterCoreServices(context, assemblies);
                engineBuilder.RegisterType<GuppyEngine>().As<IGuppyEngine>().SingleInstance();

                // Automatically invoke all loaded GuppyConfigurationAttribute instances
                IAttributeService<object, GuppyConfigurationAttribute> typeAttributes = assemblies.GetAttributes<GuppyConfigurationAttribute>(true);
                foreach ((Type type, GuppyConfigurationAttribute[] attributes) in typeAttributes)
                {
                    foreach (GuppyConfigurationAttribute attribute in attributes)
                    {
                        attribute.TryConfigure(boot, engineBuilder, type);
                    }
                }

                // Run any custom builder actions
                builder?.Invoke(engineBuilder);

                return _instance = engineBuilder.Build().Resolve<IGuppyEngine>();
            }
        }

        public static T Resolve<T>()
            where T : notnull
        {
            if (_instance is null)
            {
                throw new NotImplementedException();
            }

            return _instance.Resolve<T>();
        }

        private static void HandleAssemblyLoaded(IAssemblyService sender, Assembly assembly)
        {
            // First initialize all assembly loaders
            IEnumerable<IAssemblyLoader> loaders = assembly.GetTypes()
                .AssignableFrom<IAssemblyLoader>()
                .WithAttribute<AutoLoadAttribute>(true)
                .Select(t => Activator.CreateInstance(t) as IAssemblyLoader ?? throw new Exception());

            foreach (IAssemblyLoader loader in loaders)
            {
                loader.ConfigureAssemblies(sender);
            }
        }
        #endregion
    }
}
