using Guppy.DependencyInjection;
using Guppy.Network.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Groups
{
    public class Group : Frameable
    {
        #region Public Attributes
        public UserCollection Users { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Users = provider.GetService<UserCollection>();
        }
        #endregion
    }
}
