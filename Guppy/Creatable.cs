using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Guppy.Utilities;
using Guppy.Utilities.Delegaters;
using Guppy.Interfaces;

namespace Guppy
{
    /// <summary>
    /// A creatable class represents an object
    /// that can be created after instantiation.
    /// 
    /// The creation process is generally called
    /// manually within a factory class.
    /// </summary>
    public abstract class Creatable : ICreatable
    {
        #region Private Fields 
        private Boolean _created;
        #endregion

        #region Protected Fields
        protected ILogger logger;
        #endregion

        #region Public Attributes
        public Guid Id { get; internal set; }
        #endregion

        #region Events & Delegates
        public event EventHandler OnDisposing;
        #endregion

        public Creatable()
        {
            this.Id = Guid.NewGuid();
        }

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
            this.logger = provider.GetRequiredService<ILogger>();
        }

        public virtual void Dispose()
        {
            this.OnDisposing?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
