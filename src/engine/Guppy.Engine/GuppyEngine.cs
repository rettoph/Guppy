using Autofac;
using Autofac.Features.ResolveAnything;
using Guppy.Core;
using Guppy.Core.Common;
using Guppy.Core.Common.Contexts;
using Guppy.Core.Common.Services;
using Guppy.Core.Extensions;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Engine.Extensions.Autofac;

namespace Guppy.Engine
{
    public class GuppyEngine : IGuppyEngine, IDisposable
    {
        private readonly IGuppyScope _rootScope;

        private readonly IFiltered<IHostedService> _hostedServices;
        private readonly IFiltered<IEngineComponent> _components;

        public IGuppyContext Context { get; }
        public IEnumerable<IEngineComponent> Components => this._components;

        public GuppyEngine(GuppyContext context, Action<IGuppyScopeBuilder>? builder = null)
        {
            context.AddLibrary(typeof(GuppyEngine).Assembly);

            this._rootScope = GuppyEngine.BuildRootScope(this, context, builder);
            this._hostedServices = this._rootScope.Resolve<IFiltered<IHostedService>>();
            this._components = this._rootScope.Resolve<IFiltered<IEngineComponent>>();

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
            return this._rootScope.Resolve<T>();
        }

        #region Static
        private static readonly object _lock;
        private bool _disposed;

        static GuppyEngine()
        {
            _lock = new object();
        }

        private static IGuppyScope BuildRootScope(GuppyEngine engine, GuppyContext context, Action<IGuppyScopeBuilder>? builder = null)
        {
            lock (_lock)
            {

                // Construct a service container to store factory related services
                // Used for boot loaders, engine loaders, and related services
                IGuppyScopeBuilder bootScopeBuilder = new GuppyScopeBuilder(null);
                bootScopeBuilder.ContainerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
                bootScopeBuilder.RegisterCoreServices(context).RegisterBootServices();
                bootScopeBuilder.Register<IGuppyScope>((ILifetimeScope scope) => new GuppyScope(null, scope));

                // Construct the factory service and prepare for assembly loading.
                IGuppyScope bootScope = bootScopeBuilder.ContainerBuilder.Build().Resolve<IGuppyScope>();

                // Load assemblies
                IAssemblyService assemblies = bootScope.Resolve<IAssemblyService>();
                assemblies.Load(bootScope.Resolve<IGuppyContext>().Entry);

                // Begin boot phase 2 - call all boot attributes

                // Construct the engine container
                IGuppyScopeBuilder engineRootScopeBuilder = new GuppyScopeBuilder(bootScope);
                engineRootScopeBuilder.Register<IGuppyScope>((ILifetimeScope scope) => new GuppyScope(null, scope));

                // Run any custom builder actions
                builder?.Invoke(engineRootScopeBuilder);

                engineRootScopeBuilder.RegisterEngineServices()
                    .RegisterCoreServices(context, assemblies);

                engineRootScopeBuilder.RegisterInstance(engine).AsImplementedInterfaces().SingleInstance();

                IGuppyScope engineRootScope = engineRootScopeBuilder.ContainerBuilder.Build().Resolve<IGuppyScope>();

                // Ensure this gets resolved once
                // TODO: Fix the need for this line
                engineRootScope.Resolve<ITags>();

                return engineRootScope;
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

                    this._rootScope.Dispose();
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