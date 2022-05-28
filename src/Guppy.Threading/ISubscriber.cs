using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading
{
    public interface ISubscriber<T>
    {
        /// <summary>
        /// Process an incoming <paramref name="message"/>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Process(in T message);
    }
}
