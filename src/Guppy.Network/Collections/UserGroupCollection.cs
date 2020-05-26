using Guppy.Collections;
using Guppy.Network.Groups;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Collections
{
    /// <summary>
    /// Used by user instances to store a list of
    /// all known groups they reside in. On the server,
    /// this is every group but on the client this
    /// will only contain shared groups.
    /// </summary>
    public class UserGroupCollection : ServiceCollection<Group>
    {
        internal UserGroupCollection()
        {

        }
    }
}
