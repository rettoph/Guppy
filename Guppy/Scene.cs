﻿using Guppy.Utilities.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Collections;
using Guppy.Attributes;

namespace Guppy
{
    public class Scene : Asyncable
    {
        #region Protected Attributes
        protected LayerCollection layers { get; private set; }
        protected EntityCollection entities { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.layers = provider.GetService<LayerCollection>();
            this.entities = provider.GetService<EntityCollection>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.layers.Create<Layer>().Dispose();
            this.layers.Create<Layer>();
        }

        public override void Dispose()
        {
            base.Dispose();

            // Dispose of all self contained layers and entities
            this.layers.Dispose();
            this.entities.Dispose();
        }
        #endregion
    }
}
