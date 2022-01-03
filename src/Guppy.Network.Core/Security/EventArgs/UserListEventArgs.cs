using Guppy.Network.Enums;
using Guppy.Network.Security.Enums;
using Guppy.Network.Security.Lists;
using Guppy.Threading.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Security.EventArgs
{
    public sealed class UserListEventArgs : System.EventArgs
    {
        public readonly User User;
        public readonly UserListAction Action;

        public UserListEventArgs(User user, UserListAction action)
        {
            this.User = user;
            this.Action = action;
        }
    }
}
