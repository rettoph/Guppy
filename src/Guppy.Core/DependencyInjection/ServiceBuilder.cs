﻿using Guppy.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    public sealed class ServiceBuilder
    {
        #region Public Fields
        /// <summary>
        /// The factory this builder is linked to.
        /// </summary>
        public readonly Type Factory;

        /// <summary>
        /// The builder's numerical order. Defines
        /// when to execute the builder method when building
        /// a new service instance.
        /// </summary>
        public readonly Int32 Order;
        #endregion

        #region Private Fields
        private readonly Action<Object, ServiceProvider> _builder;
        #endregion

        #region Constructor
        internal ServiceBuilder(
            Type factory,
            Action<Object, ServiceProvider> builder,
            Int32 order)
        {
            _builder = builder;

            this.Factory = factory;
            this.Order = order;
        }
        #endregion

        #region Methods
        public void Build(Object instance, ServiceProvider provider)
            => _builder.Invoke(instance, provider);
        #endregion
    }
}
