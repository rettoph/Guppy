using Guppy.DependencyInjection;
using Guppy.Network.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists;

namespace Guppy.Network.Groups
{
    public abstract class Group : Messageable
    {
        #region Public Attributes
        public ServiceList<User> Users { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Users = provider.GetService<ServiceList<User>>();
        }
        #endregion

        #region Messageable Implementation
        protected override MessageTarget TargetType()
            => MessageTarget.Group;
        #endregion
    }
}
