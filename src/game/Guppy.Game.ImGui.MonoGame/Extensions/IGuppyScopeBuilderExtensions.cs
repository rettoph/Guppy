﻿using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Resources.Common.Extensions;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Extensions;
using Guppy.Game.ImGui.MonoGame.ResourceTypes;

namespace Guppy.Game.ImGui.MonoGame.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterGameMonoGameImGuiServices(this IGuppyScopeBuilder builder)
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