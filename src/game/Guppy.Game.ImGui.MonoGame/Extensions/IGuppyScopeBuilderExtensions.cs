using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Resources.Common.Extensions;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Extensions;
using Guppy.Game.ImGui.MonoGame.ResourceTypes;
using Guppy.Game.ImGui.MonoGame.Systems.Engine;

namespace Guppy.Game.ImGui.MonoGame.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterGameMonoGameImGuiServices(this IGuppyScopeBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterGameMonoGameImGuiServices), builder =>
            {
                builder.RegisterCommonImGuiServices();

                builder.RegisterGlobalSystem<ImGuiEventSystem>();

                builder.RegisterType<MonoGameImGuiBatch>().As<IImguiBatch>().SingleInstance();
                builder.RegisterType<ImGui>().As<IImGui>().SingleInstance();

                builder.RegisterResourceType<ImStyleResourceType>();
                builder.RegisterResourceType<TrueTypeFontResourceType>();
            });
        }
    }
}