using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IFilter
    {
        void Initialize(IServiceProvider provider);

        /// <summary>
        /// Indicates the filter should be applied to
        /// aliases of the input <see cref="Type"/>.
        /// </summary>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        bool AppliesTo(Type implementationType);

        bool Invoke(IServiceProvider provider, Type implementationType);
    }
}
