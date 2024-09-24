using Autofac;
using Autofac.Features.ResolveAnything;
using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Contexts;
using Guppy.Core.Common.Extensions.System;
using Guppy.Core.Common.Services;
using Guppy.Core.Common.Utilities;
using Guppy.Core.Extensions;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Engine.Common.Loaders;
using System.Reflection;

namespace Guppy.Engine
{
    public class GuppyEngine : IGuppyEngine, IDisposable
    {
        private readonly ILifetimeScope _container;

        private readonly IFiltered<IHostedService> _hostedServices;
        private readonly IFiltered<IEngineComponent> _components;

        public IGuppyContext Context { get; }
        public IEnumerable<IEngineComponent> Components => _components;

        public GuppyEngine(GuppyContext context, Action<ContainerBuilder>? builder = null)
        {
            _container = GuppyEngine.Build(this, context, builder);
            _hostedServices = _container.Resolve<IFiltered<IHostedService>>();
            _components = _container.Resolve<IFiltered<IEngineComponent>>();

            this.Context = context;

            CancellationTokenSource startToken = new CancellationTokenSource(5000);
            foreach (IHostedService hostedService in _hostedServices)
            {
                hostedService.StartAsync(startToken.Token);
            }
        }

        public void Dispose()
        {
            CancellationTokenSource stopToken = new CancellationTokenSource(5000);
            foreach (IHostedService hostedService in _hostedServices)
            {
                hostedService.StopAsync(stopToken.Token);
            }

            _container.Dispose();
        }

        IGuppyEngine IGuppyEngine.Start()
        {
            return this.Start();
        }

        protected virtual void Initialize()
        {
            ActionSequenceGroup<InitializeComponentSequenceGroup, IGuppyEngine>.Invoke(_components, this);
        }

        public GuppyEngine Start()
        {
            this.Initialize();

            return this;
        }

        public T Resolve<T>()
            where T : notnull
        {
            return _container.Resolve<T>();
        }

        #region Static
        private static readonly object _lock;
        private static IGuppyEngine? _instance;

        static GuppyEngine()
        {
            _lock = new object();
        }

        private static IContainer Build(GuppyEngine engine, GuppyContext context, Action<ContainerBuilder>? builder = null)
        {
            lock (_lock)
            {
                if (_instance is not null)
                {
                    throw new Exception($"{nameof(GuppyEngine)}::{nameof(Build)} - An {nameof(IGuppyEngine)} instance has already been created.");
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
                engineBuilder.RegisterInstance(engine).AsImplementedInterfaces().SingleInstance();

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

                IContainer container = engineBuilder.Build();
                StaticInstance<IContainer>.Initialize(container, true);

                return container;
            }
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
