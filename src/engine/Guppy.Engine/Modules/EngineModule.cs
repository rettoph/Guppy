using Autofac;
using Guppy.Engine.Common.Services;
using Guppy.Engine.Services;
using System.Text.Json.Serialization;

namespace Guppy.Engine.Modules
{
    public class EngineModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ObjectTextFilterService>().As<IObjectTextFilterService>().SingleInstance();
            builder.RegisterType<DefaultObjectTextFilter>().As<ObjectTextFilter>().SingleInstance();

            builder.RegisterInstance(new JsonStringEnumConverter()).As<JsonConverter>().SingleInstance();
        }
    }
}
