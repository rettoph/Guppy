using Guppy.Game.Input.Common;
using Microsoft.Xna.Framework.Input;

namespace Autofac
{
    public static partial class ContainerBuilderExtensions
    {
        public static void RegisterInput<TData>(this ContainerBuilder builder, string key, ButtonSource defaultSource, (bool, TData)[] data)
            where TData : IInput
        {
            builder.Register(p => new Button<TData>(key, defaultSource, data)).As<IButton>().SingleInstance();
        }

        public static void RegisterInput<TData>(this ContainerBuilder builder, string key, ButtonSource defaultSource, (ButtonState, TData)[] data)
            where TData : IInput
        {
            builder.Register(p => new Button<TData>(key, defaultSource, data.Select(x => (x.Item1 == ButtonState.Pressed, x.Item2)).ToArray())).As<IButton>().SingleInstance();
        }

        public static void RegisterInput<TData>(this ContainerBuilder builder, string key, ButtonSource defaultSource, (KeyState, TData)[] data)
            where TData : IInput
        {
            builder.Register(p => new Button<TData>(key, defaultSource, data.Select(x => (x.Item1 == KeyState.Down, x.Item2)).ToArray())).As<IButton>().SingleInstance();
        }

    }
}