using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using Autofac.Core.Registration;
using Guppy.Core.Common.Providers;
using Guppy.Core.Middleware;
using Guppy.Core.Services;
using Serilog;

namespace Guppy.Core.Modules
{
    /// <summary>
    /// Automatically inject ILogger instances with context
    /// based on the consuming service type.
    /// 
    /// Inspired by https://github.com/nblumhardt/autofac-serilog-integration/blob/dev/src/AutofacSerilogIntegration/SerilogMiddleware.cs
    /// </summary>
    internal sealed class LoggerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<LoggerService>().As<ILoggerService>().InstancePerLifetimeScope();
            builder.RegisterType<LogLevelService>().As<ILogLevelService>().SingleInstance();

            builder.Register<ILogger>(context => context.Resolve<ILoggerService>().GetOrCreate()).InstancePerLifetimeScope();
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

            if (LoggerModule.UsesLogger(ra) == false)
            {
                return;
            }

            registration.PipelineBuilding += (_, pipline) =>
            {
                pipline.Use(new LoggerMiddleware(registration.Activator.LimitType));
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