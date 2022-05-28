using Guppy.EntityComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public class NetTarget
    {
        public readonly ushort Id;
    }

    public sealed class NetTarget<TEntity>
        where TEntity : IEntity
    {
    }
}
