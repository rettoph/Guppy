using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using Autofac.Core.Registration;
using Guppy.Core.Logging.Common;
using Guppy.Core.Logging.Middleware;

namespace Guppy.Core.Logging.Modules
{
    /// <summary>
    /// Automatically inject ILogger instances with context
    /// based on the consuming service type.
    /// 
    /// Inspired by https://github.com/nblumhardt/autofac-serilog-integration/blob/dev/src/AutofacSerilogIntegration/SerilogMiddleware.cs
    /// </summary>
    internal sealed class LoggingModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }

        protected override void AttachToComponentRegistration(
            IComponentRegistryBuilder componentRegistry,
            IComponentRegistration registration)
        {
            base.AttachToComponentRegistration(componentRegistry, registration);

            if (registration.Services.OfType<TypedService>().Any(ts => ts.ServiceType.IsAssignableTo<ILogger>()))
            {
                return;
            }

            if (registration.Activator is not ReflectionActivator ra)
            {
                return;
            }

            foreach (LoggerParameterContext loggerParameterContext in LoggingModule.GetLoggerParameterContexts(ra, registration))
            {
                registration.PipelineBuilding += (_, pipline) =>
                {
                    pipline.Use(new LoggingMiddleware(loggerParameterContext));
                };
            }
        }

        private static IEnumerable<LoggerParameterContext> GetLoggerParameterContexts(ReflectionActivator ra, IComponentRegistration registration)
        {
            _ = Array.Empty<ConstructorInfo>();
            ConstructorInfo[] constructors;
            try
            {
                constructors = ra.ConstructorFinder.FindConstructors(ra.LimitType);
            }
            catch (NoConstructorsFoundException)
            {
                // Not sure what to do here...
                yield break;
            }

            foreach (ConstructorInfo ctor in constructors)
            {
                foreach (ParameterInfo param in ctor.GetParameters())
                {
                    if (param.ParameterType == typeof(ILogger))
                    {
                        yield return new LoggerParameterContext(registration.Activator.LimitType, param.ParameterType);
                    }

                    if (param.ParameterType.IsAssignableTo<ILogger>() == true
                        && param.ParameterType.IsGenericType == true
                        && param.ParameterType.GetGenericTypeDefinition() == typeof(ILogger<>))
                    {
                        yield return new LoggerParameterContext(param.ParameterType.GenericTypeArguments[0], param.ParameterType);
                    }
                }
            }
        }
    }
}