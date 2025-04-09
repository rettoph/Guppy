using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Assets.Common.Configuration;
using Guppy.Core.Assets.Common.Constants;
using Guppy.Core.Assets.Common.Interfaces;
using Guppy.Core.Assets.Common.Internals;
using Guppy.Core.Assets.Common.AssetTypes;

namespace Guppy.Core.Assets.Common.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterAssetPack(this IGuppyRootBuilder builder, AssetPackConfiguration configuration)
        {
            builder.RegisterInstance(configuration);
            return builder;
        }

        public static IGuppyRootBuilder RegisterAssetType<T>(this IGuppyRootBuilder builder)
            where T : IAssetType
        {
            builder.RegisterType<T>().As<IAssetType>().SingleInstance();

            return builder;
        }

        /// <summary>
        /// Register resource values at runtime.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IGuppyRootBuilder RegisterAsset<T>(this IGuppyRootBuilder builder, AssetKey<T> key, T value, string localization = Localization.en_US)
            where T : notnull
        {
            builder.RegisterInstance(new RuntimeAsset<T>(key, value, localization)).As<IRuntimeAsset>();

            return builder;
        }

        /// <summary>
        /// Register resource values at runtime.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IGuppyRootBuilder RegisterAsset<T>(this IGuppyRootBuilder builder, string key, T value, string localization = Localization.en_US)
            where T : notnull
        {
            builder.RegisterInstance(new RuntimeAsset<T>(AssetKey<T>.Get(key), value, localization)).As<IRuntimeAsset>();

            return builder;
        }
    }
}