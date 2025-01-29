using Autofac;
using Autofac.Features.ResolveAnything;
using Guppy.Core;
using Guppy.Core.Common;
using Guppy.Core.Common.Constants;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Services;
using Guppy.Core.Extensions;
using Guppy.Core.Services;
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

        public IEnumerable<IEngineComponent> Components => this._components;

        public GuppyEngine(
            IEnumerable<IEnvironmentVariable> environment,
            Action<IGuppyScopeBuilder>? builder = null)
        {
            this._rootScope = GuppyEngine.BuildRootScope(this, environment, builder);
            this._hostedServices = this._rootScope.ResolveService<IFiltered<IHostedService>>();
            this._components = this._rootScope.ResolveService<IFiltered<IEngineComponent>>();

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
            return this._rootScope.ResolveService<T>();
        }

        #region Static
        private static readonly object _lock;
        private bool _disposed;

        static GuppyEngine()
        {
            _lock = new object();
        }

        private static IGuppyScope BuildRootScope(
            GuppyEngine engine,
            IEnumerable<IEnvironmentVariable> environment,
            Action<IGuppyScopeBuilder>? builder = null)
        {
            lock (_lock)
            {
                EnvironmentVariableService environmentVariableService = new(environment);

                // Construct a service container to store factory related services
                // Used for boot loaders, engine loaders, and related services
                IGuppyScopeBuilder bootScopeBuilder = new GuppyScopeBuilder(GuppyScopeTypeEnum.Boot, null);
                bootScopeBuilder.ContainerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
                bootScopeBuilder.RegisterCoreServices(environmentVariableService).RegisterBootServices();

                // Construct the factory service and prepare for assembly loading.
                IGuppyScope bootScope = bootScopeBuilder.Build();

                // Load assemblies
                IAssemblyService assemblies = bootScope.ResolveService<IAssemblyService>();
                assemblies.Load(environmentVariableService.Get<GuppyVariables.Global.EntryAssembly>().Value);

                // Begin boot phase 2 - call all boot attributes

                // Construct the engine container
                IGuppyScopeBuilder engineRootScopeBuilder = new GuppyScopeBuilder(GuppyScopeTypeEnum.Root, bootScope);

                // Run any custom builder actions
                builder?.Invoke(engineRootScopeBuilder);

                engineRootScopeBuilder.RegisterEngineServices()
                    .RegisterCoreServices(environmentVariableService, assemblies);

                engineRootScopeBuilder.RegisterInstance(engine).AsImplementedInterfaces().SingleInstance();

                IGuppyScope engineRootScope = engineRootScopeBuilder.Build();

                // Ensure this gets resolved once
                // TODO: Fix the need for this line
                engineRootScope.ResolveService<ITags>();

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