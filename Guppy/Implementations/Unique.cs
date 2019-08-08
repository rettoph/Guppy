using Guppy.Interfaces;
using Guppy.Utilities.Delegaters;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Guppy.Implementations
{
    public class Unique : IUnique
    {
        #region Private Fields
        private Boolean _created;
        #endregion

        #region Protected Attributes
        protected IServiceProvider provider { get; private set; }
        protected ILogger logger { get; set; }
        #endregion

        #region Public Attributes
        public Guid Id { get; protected set; }
        public EventDelegater Events { get; private set; }
        #endregion

        #region Constructor
        public Unique()
        {
            _created = false;
        }
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
            this.Events.TryRegisterDelegate<DateTime>("disposing");
            this.Events.TryRegisterDelegate<Guid>("changed:id");
        }

        protected virtual void PostCreate(IServiceProvider provider)
        {
            // 
        }

        public void Dispose()
        {
            this.Events.Invoke<DateTime>("disposing", DateTime.Now);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Update the current objects id & trigger an event
        /// </summary>
        /// <param name="id"></param>
        public virtual void SetId(Guid id)
        {
            if(id != this.Id)
            {
                this.Id = id;
                this.Events.Invoke<Guid>("changed:id", id);
            }
        }
        #endregion
    }
}
