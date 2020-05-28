﻿using Guppy.DependencyInjection;
using Guppy.Network.Collections;
using Guppy.Network.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Groups
{
    public abstract class Group : Messageable
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


        #region Messageable Implementation
        protected override MessageTarget TargetType()
            => MessageTarget.Group;
        #endregion
    }
}
