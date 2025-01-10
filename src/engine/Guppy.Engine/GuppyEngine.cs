﻿using Autofac;
using Autofac.Core;
using Autofac.Features.ResolveAnything;
using Guppy.Core.Common;
using Guppy.Core.Common.Contexts;
using Guppy.Core.Common.Services;
using Guppy.Core.Common.Utilities;
using Guppy.Core.Extensions;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Engine.Common.Extensions.Autofac;
using Guppy.Engine.Extensions.Autofac;

namespace Guppy.Engine
{
    public class GuppyEngine : IGuppyEngine, IDisposable
    {
        private readonly ILifetimeScope _container;

        private readonly IFiltered<IHostedService> _hostedServices;
        private readonly IFiltered<IEngineComponent> _components;

        public IGuppyContext Context { get; }
        public IEnumerable<IEngineComponent> Components => this._components;

        public GuppyEngine(GuppyContext context, Action<ContainerBuilder>? builder = null)
        {
            this._container = GuppyEngine.Build(this, context, builder);
            this._hostedServices = this._container.Resolve<IFiltered<IHostedService>>();
            this._components = this._container.Resolve<IFiltered<IEngineComponent>>();

            this.Context = context;

            CancellationTokenSource startToken = new(5000);
            foreach (IHostedService hostedService in this._hostedServices)
            {
                hostedService.StartAsync(startToken.Token);
            }
        }

        IGuppyEngine IGuppyEngine.Start()
        {
            return this.Start();
        }

        protected virtual void Initialize()
        {
            ActionSequenceGroup<InitializeComponentSequenceGroupEnum, IGuppyEngine>.Invoke(this._components, false, this);
        }

        public GuppyEngine Start()
        {
            this.Initialize();

            return this;
        }

        public T Resolve<T>()
            where T : notnull
        {
            return this._container.Resolve<T>();
        }

        #region Static
        private static readonly object _lock;
        private bool _disposed;

        static GuppyEngine()
        {
            _lock = new object();
        }

        private static IContainer Build(GuppyEngine engine, GuppyContext context, Action<ContainerBuilder>? builder = null)
        {
            lock (_lock)
            {
                // Construct a service container to store factory related services
                // Used for boot loaders, engine loaders, and related services
                ContainerBuilder bootBuilder = new();
                bootBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
                bootBuilder.RegisterCoreServices(context).RegisterBootServices();

                // Construct the factory service and prepare for assembly loading.
                IContainer boot = bootBuilder.Build();

                // Load assemblies
                IAssemblyService assemblies = boot.Resolve<IAssemblyService>();
                assemblies.Load(boot.Resolve<IGuppyContext>().Entry);

                // Begin boot phase 2 - call all boot attributes

                // Construct the engine container
                ContainerBuilder engineBuilder = new();

                // Run any custom builder actions
                builder?.Invoke(engineBuilder);

                engineBuilder.RegisterEngineServices()
                    .RegisterCoreServices(context, assemblies);

                engineBuilder.RegisterInstance(engine).AsImplementedInterfaces().SingleInstance();

                // Automatically load all modules found within every defined assembly
                foreach (Type moduleType in assemblies.GetTypes<IModule>())
                {
                    engineBuilder.RegisterModule(moduleType, boot);
                }

                IContainer container = engineBuilder.Build();
                StaticInstance<IContainer>.Initialize(container, true);

                return container;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    CancellationTokenSource stopToken = new(5000);
                    foreach (IHostedService hostedService in this._hostedServices)
                    {
                        hostedService.StopAsync(stopToken.Token);
                    }

                    this._container.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                this._disposed = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~GuppyEngine()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}