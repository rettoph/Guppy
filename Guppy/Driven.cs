using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Utilities;
using Guppy.Extensions.Collection;
using Guppy.Collections;

namespace Guppy
{
    public abstract class Driven : Frameable, IDriven
    {
        #region Public Attributes
        public FrameableCollection<IDriver> Drivers { get; set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.Drivers.ForEach(d => d.TryCreate(provider));
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            this.Drivers.ForEach(d => d.TryPreInitialize());
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.Drivers.ForEach(d => d.TryInitialize());
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            this.Drivers.ForEach(d => d.TryPostInitialize());
        }

        public override void Dispose()
        {
            base.Dispose();

            this.Drivers.ForEach(d => d.Dispose());
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Draw all drivers...
            this.Drivers.TryDraw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update all drivers...
            this.Drivers.TryUpdate(gameTime);
        }
        #endregion
    }
}
