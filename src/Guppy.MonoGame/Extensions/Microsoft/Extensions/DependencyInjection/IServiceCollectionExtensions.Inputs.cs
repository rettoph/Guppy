using Guppy;
using Guppy.Common;
using Guppy.MonoGame;
using Guppy.MonoGame.Definitions;
using Guppy.MonoGame.Structs;
using Guppy.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddInput<TData>(this IServiceCollection services, string key, InputSource defaultSource, (bool, TData)[] data)
            where TData : IMessage
        {
            return services.AddSingleton(typeof(IInput), new Input<TData>(key, defaultSource, data));
        }
    }
}
