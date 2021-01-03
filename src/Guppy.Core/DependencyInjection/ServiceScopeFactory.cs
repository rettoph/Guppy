using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    public sealed class ServiceScopeFactory : IServiceScopeFactory, IDisposable
    {
        private ServiceProvider _parent;
        private List<ServiceScope> _scopes;

        internal ServiceScopeFactory(ServiceProvider parent)
        {
            _parent = parent;
            _scopes = new List<ServiceScope>();
        }

        public ServiceScope CreateScope()
        {
            var scope = new ServiceScope(_parent);
            _scopes.Add(scope);
            return scope;
        }

        #region IServiceScopeFactory Implementation
        IServiceScope IServiceScopeFactory.CreateScope()
            => this.CreateScope();
        #endregion

        #region IDisposeable Implementation
        public void Dispose()
        {
            _scopes.ForEach(s => s.Dispose());
            _scopes.Clear();
        }
        #endregion
    }
}
