using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    internal sealed class ServiceScopeFactory : IServiceScopeFactory, IDisposable
    {
        private ServiceProvider _parent;
        private List<IServiceScope> _scopes;

        public ServiceScopeFactory(ServiceProvider parent)
        {
            _parent = parent;
            _scopes = new List<IServiceScope>();
        }

        public IServiceScope CreateScope()
        {
            var scope = _parent.GetService<IServiceScope>();
            _scopes.Add(scope);
            return scope;
        }

        public void Dispose()
        {
            foreach (IServiceScope scope in _scopes)
                scope.Dispose();

            _scopes.Clear();
        }
    }
}
