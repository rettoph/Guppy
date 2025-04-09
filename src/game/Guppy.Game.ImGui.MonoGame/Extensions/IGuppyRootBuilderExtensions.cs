using Autofac;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Assets.Common.Extensions;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Extensions;
using Guppy.Game.ImGui.MonoGame.AssetTypes;
using Guppy.Game.ImGui.MonoGame.Systems.Engine;

namespace Guppy.Game.ImGui.MonoGame.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterGameMonoGameImGuiServices(this IGuppyRootBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterGameMonoGameImGuiServices), builder =>
            {
                builder.RegisterCommonImGuiServices();

                builder.RegisterGlobalSystem<ImGuiEventSystem>();
                builder.RegisterGlobalSystem<InitializeImGuiBatchSystem>();

                builder.RegisterType<MonoGameImGuiBatch>().AsSelf().As<IImguiBatch>().SingleInstance();
                builder.RegisterType<ImGui>().As<IImGui>().SingleInstance();

                builder.RegisterAssetType<ImStyleAssetType>();
                builder.RegisterAssetType<TrueTypeFontAssetType>();
            });
        }
    }
}