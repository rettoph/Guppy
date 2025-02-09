using Guppy.Core.Common;
using Guppy.Game.Input.Common;
using Microsoft.Xna.Framework.Input;

namespace Guppy.Core.Commands.Common
{
    public static partial class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterInput<TData>(this IGuppyScopeBuilder builder, string key, ButtonSource defaultSource, (bool, TData)[] data)
            where TData : IInputMessage
        {
            builder.Register(p => new Button<TData>(key, defaultSource, data)).As<IButton>().SingleInstance();
            return builder;
        }

        public static IGuppyScopeBuilder RegisterInput<TData>(this IGuppyScopeBuilder builder, string key, ButtonSource defaultSource, (ButtonState, TData)[] data)
            where TData : IInputMessage
        {
            builder.Register(p => new Button<TData>(key, defaultSource, data.Select(x => (x.Item1 == ButtonState.Pressed, x.Item2)).ToArray())).As<IButton>().SingleInstance();
            return builder;
        }

        public static IGuppyScopeBuilder RegisterInput<TData>(this IGuppyScopeBuilder builder, string key, ButtonSource defaultSource, (KeyState, TData)[] data)
            where TData : IInputMessage
        {
            builder.Register(p => new Button<TData>(key, defaultSource, data.Select(x => (x.Item1 == KeyState.Down, x.Item2)).ToArray())).As<IButton>().SingleInstance();
            return builder;
        }
    }
}