﻿using DotNetUtils.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    public sealed class ComponentConfiguration
    {
        #region Public Properties
        /// <summary>
        /// The <see cref="ServiceConfiguration{TServiceProvider}"/> bound to the current component configuration's
        /// <see cref="ComponentServiceName"/>.
        /// </summary>
        public readonly ServiceConfiguration<GuppyServiceProvider> ComponentServiceConfiguration;

        /// <summary>
        /// All registered entity <see cref="ServiceConfiguration{TServiceProvider}"/> bound to this
        /// configuration.
        /// </summary>
        public readonly ServiceConfiguration<GuppyServiceProvider>[] EntityServiceConfigurations;

        public readonly Int32 Order;

        /// <summary>
        /// Every registered filter applicable to the current component configuration.
        /// </summary>
        public readonly ComponentFilter[] Filters;
        #endregion

        #region Constructors
        public ComponentConfiguration(
            ServiceConfiguration<GuppyServiceProvider>  componentServiceConfiguration,
            ServiceConfiguration<GuppyServiceProvider>[] entityServiceConfigurations, 
            Int32 order, 
            ComponentFilter[] filters)
        {
            this.ComponentServiceConfiguration = componentServiceConfiguration;
            this.EntityServiceConfigurations = entityServiceConfigurations;
            this.Order = order;
            this.Filters = filters;  
        }
        #endregion

        #region Helper Methods
        public Boolean CheckFilters(IEntity entity, GuppyServiceProvider provider)
        {
            foreach(ComponentFilter filter in this.Filters)
            {
                if(!filter.Method(entity, provider, this.ComponentServiceConfiguration))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
