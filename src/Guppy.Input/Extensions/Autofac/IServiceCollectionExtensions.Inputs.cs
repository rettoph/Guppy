using Autofac;
using Guppy;
using Guppy.Common;
using Guppy.Input;
using Guppy.Providers;
using Microsoft.Xna.Framework.Input;

namespace Autofac
{
    public static partial class IServiceCollectionExtensions
    {
        public static void AddInput<TData>(this ContainerBuilder builder, string key, ButtonSource defaultSource, (bool, TData)[] data)
            where TData : IMessage
        {
            builder.Register(p => new Button<TData>(key, defaultSource, data)).As<IButton>().InstancePerLifetimeScope();
        }

        public static void AddInput<TData>(this ContainerBuilder builder, string key, ButtonSource defaultSource, (ButtonState, TData)[] data)
            where TData : IMessage
        {
            builder.Register(p => new Button<TData>(key, defaultSource, data.Select(x => (x.Item1 == ButtonState.Pressed, x.Item2)).ToArray())).As<IButton>().InstancePerLifetimeScope();
        }
        
        public static void AddInput<TData>(this ContainerBuilder builder, string key, ButtonSource defaultSource, (KeyState, TData)[] data)
            where TData : IMessage
        {
            builder.Register(p => new Button<TData>(key, defaultSource, data.Select(x => (x.Item1 == KeyState.Down, x.Item2)).ToArray())).As<IButton>().InstancePerLifetimeScope();
        }

    }
}
