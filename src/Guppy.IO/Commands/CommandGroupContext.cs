using Guppy.IO.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands
{
    public abstract class CommandGroupContext : ICommandGroupContext
    {
        #region Private Attributes
        private Dictionary<String, ICommandGroupContext> _groups;
        #endregion

        #region Public Attributes
        /// <inheritdoc />
        public ICommandGroupContext ParentContext { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<String, ICommandGroupContext> Groups => _groups;

        /// <inheritdoc />
        public String FullName => (this.ParentContext == default(ICommandGroupContext) ? "" : this.ParentContext.FullName + ' ') + this.Name;

        /// <inheritdoc />
        public abstract String Name { get; }

        /// <inheritdoc />
        public abstract String Description { get; }
        #endregion

        #region Constructor
        public CommandGroupContext()
        {
            _groups = new Dictionary<String, ICommandGroupContext>();
        }
        #endregion

        #region Helper Methods
        public void Add(ICommandGroupContext context)
        {
            if(context.TrySetParent(this))
                _groups.Add(context.Name, context);
        }

        public void Remove(ICommandGroupContext context)
        {
            if(this.Groups.ContainsKey(context.Name) && context.TrySetParent(null))
                _groups.Remove(context.Name);
        }

        private void SetParent(ICommandGroupContext parentContext)
        {
            this.ParentContext = parentContext;
        }

        Boolean ICommandGroupContext.TrySetParent(ICommandGroupContext parentContext)
        {
            if (this.ParentContext == default(ICommandGroupContext) || parentContext == default(ICommandGroupContext))
            {
                this.SetParent(parentContext);
                return true;
            }
            else
            { // Trying to set parent without unsetting previous parent first. 
                throw new Exception("Unset the pre existing parent first.");
            }
        }
        #endregion
    }
}
