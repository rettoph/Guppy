using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Resources.Common.Extensions.Autofac;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Extensions;
using Guppy.Game.ImGui.MonoGame.ResourceTypes;

namespace Guppy.Game.ImGui.MonoGame.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterGameMonoGameImGuiServices(this ContainerBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterGameMonoGameImGuiServices), builder =>
            {
                builder.RegisterCommonImGuiServices();

                builder.RegisterType<MonoGameImGuiBatch>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<ImGui>().As<IImGui>().SingleInstance();

                builder.RegisterResourceType<ImStyleResourceType>();
                builder.RegisterResourceType<TrueTypeFontResourceType>();
            });
        }
    }
}