using Guppy.Network.Builders;
using Guppy.Network.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Ensure that a list of <paramref name="items"/> all have a unique id, if they are null.
        /// </summary>
        /// <typeparam name="TDynamicId"></typeparam>
        /// <param name="items"></param>
        /// <param name="startFrom"></param>
        /// <returns></returns>
        public static IEnumerable<TDynamicId> AutoIncrementIds<TDynamicId>(this IEnumerable<TDynamicId> items, UInt16 startFrom = 0)
            where TDynamicId : DynamicIdConfigurationBuilder<TDynamicId>
        {
            UInt16 autoIncrememntId = startFrom;

            foreach (TDynamicId dynamicIdItem in items)
            {
                if (dynamicIdItem.Id is null)
                {
                    while (items.Any(pc => pc.Id == autoIncrememntId))
                    {
                        autoIncrememntId++;
                    }

                    dynamicIdItem.SetId(autoIncrememntId++);
                }
            }

            return items;
        }

        /// <summary>
        /// Determin the DynamicId Size of a given enumerable of DynanicIds
        /// </summary>
        /// <typeparam name="TDynamicId"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DynamicIdSize GetIdSize<TDynamicId>(this IEnumerable<TDynamicId> items)
            where TDynamicId : DynamicIdConfigurationBuilder<TDynamicId>
        {
            UInt16 maxPacketId = items.Max(di => di.Id ?? 0);
            return maxPacketId switch
            {
                <= Byte.MaxValue => DynamicIdSize.OneByte,
                _ => DynamicIdSize.TwoBytes
            };
        }
    }
}
