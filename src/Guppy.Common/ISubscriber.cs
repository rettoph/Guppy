using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface ISubscriber
    {

    }

    public interface ISubscriber<T> : ISubscriber
        where T : IMessage
    {
        /// <summary>
        /// Consume an incoming <paramref name="message"/>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        void Process(in T message);
    }
}
