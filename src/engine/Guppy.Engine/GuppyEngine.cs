using Autofac;
using Autofac.Features.ResolveAnything;
using Guppy.Core;
using Guppy.Core.Common;
using Guppy.Core.Common.Constants;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Services;
using Guppy.Core.Common.Utilities;
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

        public IEnumerable<IEngineComponent> Components => this._components;

        public GuppyEngine(
            GuppyEnvironment environment,
            Action<IGuppyScopeBuilder>? builder = null)
        {
            this._rootScope = GuppyEngine.BuildRootScope(this, environment.ToDictionary(), builder);
            this._hostedServices = this._rootScope.Resolve<IFiltered<IHostedService>>();
            this._components = this._rootScope.Resolve<IFiltered<IEngineComponent>>();

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

        private static IGuppyScope BuildRootScope(
            GuppyEngine engine,
            Dictionary<Type, IEnvironmentVariable> environment,
            Action<IGuppyScopeBuilder>? builder = null)
        {
            lock (_lock)
            {
                // Tweak the libraries variable to ensure required guppy libraries are registered...
                environment[typeof(GuppyEnvironmentVariables.LibraryAssemblies)] = GuppyEnvironmentVariables.LibraryAssemblies.Create(
                    environment[typeof(GuppyEnvironmentVariables.LibraryAssemblies)]
                        .As<GuppyEnvironmentVariables.LibraryAssemblies>().Value
                        .Concat([
                            typeof(GuppyEngine).Assembly,
                            typeof(IGuppyEngine).Assembly
                        ]).ToArray());

                // Construct a service container to store factory related services
                // Used for boot loaders, engine loaders, and related services
                IGuppyScopeBuilder bootScopeBuilder = new GuppyScopeBuilder(environment, GuppyScopeTypeEnum.Boot, null);
                bootScopeBuilder.ContainerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
                bootScopeBuilder.RegisterCoreServices().RegisterBootServices();

                // Construct the factory service and prepare for assembly loading.
                IGuppyScope bootScope = bootScopeBuilder.Build();

                // Load assemblies
                IAssemblyService assemblies = bootScope.Resolve<IAssemblyService>();
                assemblies.Load(bootScope.Resolve<GuppyEnvironment>().Get<GuppyEnvironmentVariables.EntryAssembly>().Value);

                // Begin boot phase 2 - call all boot attributes

                // Construct the engine container
                Dictionary<Type, IEnvironmentVariable> engineRootEnvironment = bootScope.Resolve<GuppyEnvironment>().ToDictionary();
                IGuppyScopeBuilder engineRootScopeBuilder = new GuppyScopeBuilder(engineRootEnvironment, GuppyScopeTypeEnum.Root, bootScope);

                // Run any custom builder actions
                builder?.Invoke(engineRootScopeBuilder);

                engineRootScopeBuilder.RegisterEngineServices()
                    .RegisterCoreServices(assemblies);

                engineRootScopeBuilder.RegisterInstance(engine).AsImplementedInterfaces().SingleInstance();

                IGuppyScope engineRootScope = engineRootScopeBuilder.Build();

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