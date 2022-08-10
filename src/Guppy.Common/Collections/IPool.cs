using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Guppy.Common.Collections
{
    public interface IPool<T>
        where T : class
    {
        /// <summary>
        /// Determin whether or not the current pool contains any items
        /// </summary>
        /// <returns></returns>
        bool Any();

        /// <summary>
        /// Attempt to extract an instance from the pool
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        bool TryPull([MaybeNullWhen(false)] out T instance);

        /// <summary>
        /// If the internal pool is not yet at max capacity,
        /// return the recieved instance into the pool.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        bool TryReturn(T instance);

        /// <summary>
        /// A counf of how many items are within the pool.
        /// </summary>
        /// <returns></returns>
        int Count();
    }
}
