using Guppy.Collections;
using Guppy.Network.Groups;
using Guppy.Network.Peers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Collections
{
    public class GroupCollection : UniqueObjectCollection<Group>
    {
        private Func<Guid, Group> _groupFactory;

        public GroupCollection(Func<Guid, Group> groupFactory)
        {
            _groupFactory = groupFactory;
        }

        /// <summary>
        /// Get a pre-existing group
        /// or create a new one based
        /// on a reference id value
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Group GetOrCreateById(Guid id)
        {
            var getGroup = this.GetById(id);

            if(getGroup == null)
            { // If no group was found then create a new one...
                var newGroup = _groupFactory.Invoke(id);
                this.Add(newGroup);

                return newGroup;
            }
            else
            { // Otherwise return the found group...
                return getGroup;
            }
        }
    }
}
