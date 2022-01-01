using Guppy.Threading.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Security.EventArgs
{
    public sealed class UserEventArgs : System.EventArgs, IData
    {
        public readonly User User;

        public UserEventArgs(User user)
        {
            this.User = user;
        }
    }
}
