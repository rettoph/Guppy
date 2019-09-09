using Guppy.Collections;
using Guppy.Network.Factories;
using Guppy.Network.Groups;
using Guppy.Network.Peers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Collections
{
    public class GroupCollection : CreatableCollection<Group>
    {
        #region Private Fields
        private GroupFactory _factory;
        private Type _groupType;
        #endregion

        #region Constructor
        public GroupCollection(Peer peer, GroupFactory factory, IServiceProvider provider) : base(provider)
        {
            _factory = factory;
            _groupType = peer.GroupType();
        }
        #endregion

        #region Create Methods
        public Group GetOrCreateById(Guid id)
        {
            var group = this.GetById<Group>(id);

            if (group == default(Group))
            {
                group = _factory.Build<Group>(_groupType, g =>
                {
                    g.SetId(id);
                });

                this.Add(group);
            }

            return group;
        }
        #endregion
    }
}
