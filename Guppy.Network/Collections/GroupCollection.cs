using Guppy.Collections;
using Guppy.Network.Configurations;
using Guppy.Network.Factories;
using Guppy.Network.Groups;
using Guppy.Network.Peers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Collections
{
    public class GroupCollection : UniqueObjectCollection<Group>
    {
        private GroupFactory _factory;

        public GroupCollection(GroupFactory factory)
        {
            _factory = factory;
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
            var group = this.GetById(id);

            if(group == null)
            { // If no group was found then create a new one...
                group = _factory.Create(id);
                this.Add(group);
            }

            return group;
        }
    }
}
