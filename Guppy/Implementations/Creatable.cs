using Guppy.Interfaces;
using Guppy.Utilities.Delegaters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Implementations
{
    /// <summary>
    /// An object that can be created. This is commonaly used for 
    /// reuable items. The create methods can only be called once.
    /// 
    /// Included inside the cretable is the EventDelegater. This allows
    /// for the creation and invocation of custom events based on a string
    /// identifier.
    /// </summary>
    public class Creatable : IDisposable
    {
        #region Private Fields
        private Boolean _created;
        #endregion

        #region Protected Attributes
        protected IServiceProvider provider { get; private set; }
        protected ILogger logger { get; private set; }
        #endregion

        #region Public Attributes
        public Guid Id { get; private set; }
        public EventDelegater Events { get; private set; }
        #endregion

        #region Constructor
        public Creatable()
        {
            _created = false;
        }
        #endregion

        #region Lifecycle Methods
        public void TryCreate(IServiceProvider provider)
        {
            if (_created)
                throw new Exception("Unable to create object. TryCreate() has already been called!");

            this.PreCreate(provider);
            this.Create(provider);
            this.PostCreate(provider);

            _created = true;
        }

        public virtual void Dispose()
        {
            this.Events.Invoke<Creatable>("disposing", this, this);
            this.Events.Dispose();
        }

        protected virtual void PreCreate(IServiceProvider provider)
        {
            this.provider = provider;
            this.logger = provider.GetService<ILogger>();
            this.Events = new EventDelegater(this.logger);
            this.Id = Guid.NewGuid();
        }

        protected virtual void Create(IServiceProvider provider)
        {
            this.Events.Register<Creatable>("disposing"); 
        }

        protected virtual void PostCreate(IServiceProvider provider)
        {
            // 
        }
        #endregion

        #region Helper Methods
        public virtual void SetId(Guid id)
        {
            if(this.Id != id)
            {
                this.Id = id;
            }
        }
        #endregion
    }
}
