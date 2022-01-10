using Guppy.EntityComponent;
using Guppy.Threading.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading
{
    public abstract class DataProcessor<TMessage> : Service, IDataProcessor<TMessage>
        where TMessage : class, IData
    {
        public abstract Boolean Process(TMessage message);
    }
}
