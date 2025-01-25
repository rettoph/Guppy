﻿using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Game.Input.Components;
using Guppy.Game.Input.Services;

namespace Guppy.Game.Input.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterGameInputServices(this IGuppyScopeBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterGameInputServices), builder =>
            {
                builder.RegisterType<KeyboardButtonService>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<MouseButtonService>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<CursorService>().AsImplementedInterfaces().SingleInstance();

                builder.RegisterType<InputService>().AsImplementedInterfaces().SingleInstance();

                builder.RegisterType<ButtonPublishComponent>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<MouseCursorPublishComponent>().AsImplementedInterfaces().SingleInstance();
            });
        }
    }
}