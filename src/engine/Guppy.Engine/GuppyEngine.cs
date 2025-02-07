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
using Guppy.Engine.Extensions;

namespace Guppy.Engine
{
    public class GuppyEngine : IGuppyEngine, IDisposable
    {
        private readonly IGuppyScope _globalScope;

        public IGlobalSystemService Systems { get; }

        public GuppyEngine(
            IEnumerable<IEnvironmentVariable> environment,
            Action<IGuppyScopeBuilder>? builder = null)
        {
            this._globalScope = GuppyEngine.BuildRootScope(this, environment, builder);

            this.Systems = this._globalScope.Resolve<IGlobalSystemService>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    ActionSequenceGroup<DeinitializeSequenceGroupEnum, IGuppyEngine>.Invoke(this.Systems, false, this);
                    this._globalScope.Dispose();
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

        IGuppyEngine IGuppyEngine.Start()
        {
            return this.Start();
        }

        protected virtual void Initialize()
        {
            ActionSequenceGroup<InitializeSequenceGroupEnum, IGuppyEngine>.Invoke(this.Systems, false, this);
        }

        public GuppyEngine Start()
        {
            this.Initialize();

            return this;
        }

        public T Resolve<T>()
            where T : notnull
        {
            return this._globalScope.Resolve<T>();
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
                IGuppyScopeBuilder bootScopeBuilder = new GuppyScopeBuilder(GuppyScopeTypeEnum.Boot, environmentVariableService, null);
                bootScopeBuilder.ContainerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
                bootScopeBuilder.RegisterCoreServices(environmentVariableService).RegisterBootServices();

                // Construct the factory service and prepare for assembly loading.
                IGuppyScope bootScope = bootScopeBuilder.Build();

                // Load assemblies
                IAssemblyService assemblies = bootScope.Resolve<IAssemblyService>();
                assemblies.Load(environmentVariableService.Get<GuppyCoreVariables.Global.EntryAssembly>().Value);

                // Begin boot phase 2 - call all boot attributes

                // Construct the engine container
                IGuppyScopeBuilder engineRootScopeBuilder = new GuppyScopeBuilder(GuppyScopeTypeEnum.Global, environmentVariableService, bootScope);

                // Run any custom builder actions
                builder?.Invoke(engineRootScopeBuilder);

                engineRootScopeBuilder.RegisterEngineServices()
                    .RegisterCoreServices(environmentVariableService, assemblies);

                engineRootScopeBuilder.RegisterInstance(engine).AsImplementedInterfaces().SingleInstance();

                IGuppyScope engineRootScope = engineRootScopeBuilder.Build();

                // Ensure this gets resolved once
                // TODO: Fix the need for this line
                engineRootScope.Resolve<ITags>();

                return engineRootScope;
            }
        }
        #endregion
    }
}