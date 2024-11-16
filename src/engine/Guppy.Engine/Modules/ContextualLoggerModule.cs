using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using Autofac.Core.Registration;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Engine.Common.Modules;
using Guppy.Engine.Common.Providers;
using Guppy.Engine.Middleware;
using Guppy.Engine.Providers;
using Serilog;
using System.Reflection;

namespace Guppy.Engine.Modules
{
    /// <summary>
    /// Automatically inject ILogger instances with context
    /// based on the consuming service type.
    /// 
    /// Inspired by https://github.com/nblumhardt/autofac-serilog-integration/blob/dev/src/AutofacSerilogIntegration/SerilogMiddleware.cs
    /// </summary>
    internal sealed class ContextualLoggerModule() : EngineModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ContextualLoggerProvider>().InstancePerLifetimeScope();

            builder.Configure<LoggerConfiguration>((scope, conf) =>
            {
                ILogLevelProvider logLevelProvider = scope.Resolve<ILogLevelProvider>();
                logLevelProvider.Configure(conf);
            });
        }

        protected override void AttachToComponentRegistration(
            IComponentRegistryBuilder componentRegistry,
            IComponentRegistration registration)
        {
            if (registration.Services.OfType<TypedService>().Any(ts => ts.ServiceType == typeof(ILogger)))
            {
                return;
            }

            if (registration.Activator is not ReflectionActivator ra)
            {
                return;
            }

            if (ContextualLoggerModule.UsesLogger(ra) == false)
            {
                return;
            }

            registration.PipelineBuilding += (_, pipline) =>
            {
                pipline.Use(new ContextualLoggerMiddleware(registration.Activator.LimitType));
            };
        }

        private static bool UsesLogger(ReflectionActivator ra)
        {
            try
            {
                foreach (ConstructorInfo ctor in ra.ConstructorFinder.FindConstructors(ra.LimitType))
                {
                    foreach (ParameterInfo param in ctor.GetParameters())
                    {
                        if (param.ParameterType == typeof(ILogger))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (NoConstructorsFoundException)
            {
                return false;
            }
        }
    }
}
