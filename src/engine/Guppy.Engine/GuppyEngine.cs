using Autofac;
using Autofac.Features.ResolveAnything;
using Guppy.Core.Builders;
using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Constants;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Common.Services;
using Guppy.Core.Extensions;
using Guppy.Core.Services;
using Guppy.Engine.Common;
using Guppy.Engine.Extensions;

namespace Guppy.Engine
{
    public class GuppyEngine : IGuppyEngine, IDisposable
    {
        private readonly IGuppyRoot _root;

        public IGlobalSystemService Systems { get; }

        public GuppyEngine(
            IEnumerable<IEnvironmentVariable> environment,
            Action<IGuppyRootBuilder>? build = null)
        {
            this._root = GuppyEngine.BuildRoot(this, environment, build);

            this.Systems = this._root.Resolve<IGlobalSystemService>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    ActionSequenceGroup<DeinitializeSequenceGroupEnum>.Invoke(this.Systems.GetAll(), false);
                    this._root.Dispose();
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
            ActionSequenceGroup<InitializeSequenceGroupEnum>.Invoke(this.Systems.GetAll(), false);
        }

        public GuppyEngine Start()
        {
            this.Initialize();

            return this;
        }

        public T Resolve<T>()
            where T : class
        {
            return this._root.Resolve<T>();
        }

        #region Static
        private static readonly object _lock;
        private bool _disposed;

        static GuppyEngine()
        {
            _lock = new object();
        }

        private static IGuppyRoot BuildRoot(
            GuppyEngine engine,
            IEnumerable<IEnvironmentVariable> environment,
            Action<IGuppyRootBuilder>? build = null)
        {
            lock (_lock)
            {
                EnvironmentVariableService environmentVariableService = new(environment);

                // Construct a service container to store factory related services
                // Used for boot loaders, engine loaders, and related services
                GuppyRootBuilder bootContainerBuilder = new(environment);
                bootContainerBuilder.ContainerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
                bootContainerBuilder.RegisterCoreServices().RegisterBootServices();

                // Construct the factory service and prepare for assembly loading.
                IGuppyRoot bootContainer = bootContainerBuilder.Build();

                // Load assemblies
                IAssemblyService assemblies = bootContainer.Resolve<IAssemblyService>();
                assemblies.Load(environmentVariableService.Get<GuppyCoreVariables.Environment.EntryAssembly>().Value);

                // Begin boot phase 2 - call all boot attributes

                // Construct the engine container
                GuppyRootBuilder engineRootBuilder = new(environment);

                // Run any custom builder actions
                build?.Invoke(engineRootBuilder);

                engineRootBuilder.RegisterCoreServices(assemblies).RegisterEngineServices();

                engineRootBuilder.RegisterInstance(engine).AsImplementedInterfaces().SingleInstance();

                IGuppyRoot engineRootScope = engineRootBuilder.Build();

                // Ensure this gets resolved once
                // TODO: Fix the need for this line
                engineRootScope.Resolve<ITags>();

                return engineRootScope;
            }
        }
        #endregion
    }
}