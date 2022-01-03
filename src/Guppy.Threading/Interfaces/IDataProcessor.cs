using Guppy.EntityComponent.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading.Interfaces
{
    public interface IDataProcessor<TData> : IService
        where TData : class, IData
    {
        /// <summary>
        /// Process the incoming <see cref="TData"/>
        /// message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Success status</returns>
        Boolean Process(TData message);
    }
}
