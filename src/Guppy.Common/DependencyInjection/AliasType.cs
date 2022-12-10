using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.DependencyInjection
{
    public enum AliasType
    {
        /// <summary>
        /// All aliases of the configured service will be returned as default when resolving
        /// an <see cref="IEnumerable{T}"/>
        /// </summary>
        Unfiltered,

        /// <summary>
        /// Aliase instances of the configured service will only be returned when resolving
        /// a <see cref="IFiltered{T}"/>
        /// </summary>
        Filtered
    }
}
