using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public class Pipe
    {
        #region Public Properties
        public Guid Id { get; }
        public Room Room { get; internal set; }
        #endregion
    }
}
