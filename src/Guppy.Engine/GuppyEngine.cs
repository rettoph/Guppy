using Autofac;
using Guppy.Engine.Attributes;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Autofac;
using Guppy.Engine.Common.Contexts;
using Guppy.Engine.Common.Services;
using Guppy.Engine.Extensions.Autofac;
using Guppy.Engine.Loaders;
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
        private static IContainer? _head;

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
                ContainerBuilder headBuilder = new ContainerBuilder();
                GuppyEngine.RegisterFactoryServices(headBuilder, context);

                // Construct the factory service and prepare for assembly loading.
                _head = headBuilder.Build();
                IAssemblyService assemblies = _head.Resolve<IAssemblyService>();

                assemblies.OnAssemblyLoaded += HandleAssemblyLoaded;
                assemblies.Load(_head.Resolve<IGuppyContext>().Entry);
                assemblies.OnAssemblyLoaded -= HandleAssemblyLoaded;

                // Construct the engine container
                return _instance = _head.BeginLifetimeScope(LifetimeScopeTags.BootScope, engine =>
                {
                    // Automatically invoke all loaded GuppyConfigurationAttribute instances
                    IAttributeService<object, GuppyConfigurationAttribute> typeAttributes = assemblies.GetAttributes<GuppyConfigurationAttribute>(true);
                    foreach ((Type type, GuppyConfigurationAttribute[] attributes) in typeAttributes)
                    {
                        foreach (GuppyConfigurationAttribute attribute in attributes)
                        {
                            attribute.TryConfigure(engine, type);
                        }
                    }

                    // Run the custom builder actions last
                    builder?.Invoke(engine);
                }).BeginGuppyScope(LifetimeScopeTags.EngineScope).Resolve<IGuppyEngine>();
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

        private static void RegisterFactoryServices(ContainerBuilder builder, IGuppyContext context)
        {
            builder.RegisterInstance(context).SingleInstance();
            builder.RegisterType<AssemblyService>().As<IAssemblyService>().SingleInstance();
            builder.RegisterType<GuppyEngine>().As<IGuppyEngine>().InstancePerMatchingLifetimeScope(LifetimeScopeTags.EngineScope);
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
