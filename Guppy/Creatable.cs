using Guppy.Utilities.Delegaters;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy
{
    /// <summary>
    /// A creatable class represents an object
    /// that can be created after instantiation.
    /// 
    /// The creation process is generally called
    /// manually within a factory class.
    /// </summary>
    public class Creatable : IDisposable
    {
        #region Private Fields 
        private Boolean _created;
        #endregion

        #region Protected Fields
        protected IServiceProvider provider { get; private set; }
        #endregion

        #region Public Attributes
        public Guid Id { get; internal set; }
        public EventDelegater Events { get; private set; }
        #endregion

        #region Lifecycle Methods
        public void TryCreate(IServiceProvider provider)
        {
            if (_created)
                throw new Exception("Unable to Create. Instance has already been created.");

            this.Id = Guid.NewGuid();
            this.Create(provider);

            _created = true;
        }

        protected virtual void Create(IServiceProvider provider)
        {
            this.provider = provider;

            this.Events = this.provider.GetService<EventDelegater>();
            this.Events.Register<Creatable>("disposing");
        }

        public virtual void Dispose()
        {
            this.Events.Invoke<Creatable>("disposing", this, this);
            this.Events.Dispose();
        }
        #endregion
    }
}
