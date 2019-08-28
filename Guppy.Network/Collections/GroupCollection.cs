using Guppy.Collections;
using Guppy.Network.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Collections
{
    public class GroupCollection : FrameableCollection<Group>
    {
        #region Private Fields
        private GroupFactory _factory;
        #endregion

        #region Constructor
        public GroupCollection(GroupFactory factory, IServiceProvider provider) : base(provider)
        {
            _factory = factory;
        }
        #endregion

        #region Create Methods
        public Group GetOrCreateById(Guid id)
        {
            var group = this.GetById<Group>(id);

            if (group == default(Group))
            {
                group = _factory.Build<Group>(g =>
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
