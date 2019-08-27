using Guppy.Collections;
using Guppy.Network.Factories;
using Guppy.Network.Groups;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Collections
{
    public class GroupCollection : FrameableCollection<Group>
    {
        #region Private Fields
        private GroupFactory _factory;
        private Type _groupType;
        #endregion

        #region Constructor
        public GroupCollection(Type groupType, GroupFactory factory, IServiceProvider provider) : base(provider)
        {
            _factory = factory;
            _groupType = groupType;
        }
        #endregion

        #region Create Methods
        public Group GetOrCreateById(Guid id)
        {
            var group = _factory.Build<Group>(_groupType, g =>
            {
                g.SetId(id);
            });

            this.Add(group);

            return group;
        }
        #endregion
    }
}
