using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IPool<T>
        where T : class
    {
        /// <summary>
        /// Determin whether or not the current pool contains any items
        /// </summary>
        /// <returns></returns>
        Boolean Any();

        /// <summary>
        /// Attempt to extract an instance from the pool
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        Boolean TryPull(out T instance);

        /// <summary>
        /// If the internal pool is not yet at max capacity,
        /// return the recieved instance into the pool.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        Boolean TryReturn(T instance);

        /// <summary>
        /// A counf of how many items are within the pool.
        /// </summary>
        /// <returns></returns>
        Int32 Count();
    }
}
