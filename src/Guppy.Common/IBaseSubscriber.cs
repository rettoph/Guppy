using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IBaseSubscriber<TBase>
        where TBase : IMessage
    {

    }

    public interface IBaseSubscriber<TBase, T> : IBaseSubscriber<TBase>
        where TBase : IMessage
        where T : TBase
    {
        /// <summary>
        /// Consume an incoming <paramref name="message"/>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        void Process(in Guid messageId, in T message);
    }
}
