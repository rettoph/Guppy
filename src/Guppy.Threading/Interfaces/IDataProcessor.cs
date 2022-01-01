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
        void Process(TData message);
    }
}
