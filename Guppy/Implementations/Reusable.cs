using Guppy.Enums;
using Guppy.Interfaces;
using Guppy.Utilities.Delegaters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Implementations
{
    public abstract class Reusable : Frameable, IReusable
    {
        #region Private Fields
        private Boolean _created;
        #endregion

        #region Protected Attributes
        protected internal IServiceProvider provider { get; private set; }
        protected ILogger logger { get; private set; }
        #endregion

        #region Public Attributes
        public Guid Id { get; private set; }
        public InitializationStatus Status { get; private set; }
        public EventDelegater Events { get; private set; }
        #endregion


        #region Lifecycle Methods
        public void TryCreate(IServiceProvider provider)
        {
            if (_created)
                throw new Exception($"Unable to create more than once");

            this.PreCreate(provider);
            this.Create(provider);
            this.PostCreate(provider);
            _created = true;
        }

        protected virtual void PreCreate(IServiceProvider provider)
        {
            // 
        }

        protected virtual void Create(IServiceProvider provider)
        {
            this.provider = provider;
            this.logger = provider.GetService<ILogger>();
            
            this.Id = Guid.NewGuid();
            this.Events = provider.GetService<EventDelegater>();
            this.Events.SetOwner(this);

            // Register default events
            this.Events.RegisterDelegate<DateTime>("disposing");
            this.Events.RegisterDelegate<Int32>("changed:draw-order");
            this.Events.RegisterDelegate<Int32>("changed:update-order");
            this.Events.RegisterDelegate<Boolean>("changed:visible");
            this.Events.RegisterDelegate<Boolean>("changed:enabled");
        }

        protected virtual void PostCreate(IServiceProvider provider)
        {
            // 
        }

        public override void Dispose()
        {
            base.Dispose();

            this.Events.Invoke<DateTime>("disposing", DateTime.Now);

            this.Events.Dispose();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Update the current objects id. 
        /// This can only be done if the object is not
        /// initialized.
        /// </summary>
        /// <param name="id"></param>
        public void SetId(Guid id)
        {
            if (this.InitializationStatus != InitializationStatus.NotInitialized)
                this.logger.LogWarning($"Unable to update id, status is {this.Status} but {InitializationStatus.NotInitialized} is expected.");
            else
                this.Id = id;
        }

        public void SetDrawOrder(Int32 value)
        {
            if (value != this.DrawOrder)
            {
                this.DrawOrder = value;

                this.Events.Invoke<Int32>("changed:draw-order", this.DrawOrder);
            }
        }

        public void SetUpdateOrder(Int32 value)
        {
            if (value != this.UpdateOrder)
            {
                this.UpdateOrder = value;

                this.Events.Invoke<Int32>("changed:update-order", this.UpdateOrder);
            }
        }

        public void SetVisible(Boolean value)
        {
            if (value != this.Visible)
            {
                this.Visible = value;

                this.Events.Invoke<Boolean>("changed:visible", this.Visible);
            }
        }

        public void SetEnabled(Boolean value)
        {
            if (value != this.Enabled)
            {
                this.Enabled = value;

                this.Events.Invoke<Boolean>("changed:enabled", this.Enabled);
            }
        }
        #endregion
    }
}
