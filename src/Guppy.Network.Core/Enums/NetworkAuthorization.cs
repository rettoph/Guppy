using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Enums
{
    /// <summary>
    /// Simple Enum used to designate the level of control
    /// a game should exibit when making decisions.
    /// 
    /// An example of this is the Server with full authorization will
    /// destroy a ship at zero health, while a client with partial
    /// authorization will wait for confirmation from the server before
    /// destroying the same ship.
    /// 
    /// Very often it's as simple as Server => Master, Client => Slave,
    /// but this is not always the case.
    /// </summary>
    [Flags]
    public enum NetworkAuthorization
    {
        Slave = 1,
        Master = 2
    }
}
