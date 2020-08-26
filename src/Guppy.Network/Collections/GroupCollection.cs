using Guppy.Collections;
using Guppy.DependencyInjection;
using Guppy.Network.Groups;
using Guppy.Network.Peers;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.Network.Collections
{
    public class GroupCollection : ProtectedServiceCollection<Group>
    {
        #region Private Fields
        private ServiceProvider _provider;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _provider = provider;
        }
        #endregion

        #region Factory Methods
        /// <summary>
        /// Returns a group instance 
        /// </summary>
        /// <param name="id"></param>
        public Group GetOrCreateById(Guid id)
        {
            // Return an existing group instance if one exists
            if (this.ContainsId(id))
                return this.GetById(id);

            // Create a new group instance...
            var group = _provider.GetService<Group>((g, p, c) =>
            {
                g.Id = id;
            });
            this.Add(group);
            return group;
        }
        #endregion
    }
}
