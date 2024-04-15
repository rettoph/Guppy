using Autofac;
using Autofac.Core.Lifetime;
using Guppy.Core.Common;

namespace Guppy.Core
{
    internal class Tags : ITags
    {
        private Tags? _parent;

        private ILifetimeScope _autofac;

        public bool IsRoot => _parent is null;

        public Tags(ILifetimeScope autofac)
        {
            _autofac = autofac;
            _autofac.ChildLifetimeScopeBeginning += this.HandleChildLifetimeScopeBeginning;
        }

        private void HandleChildLifetimeScopeBeginning(object? sender, LifetimeScopeBeginningEventArgs e)
        {
            e.LifetimeScope.Resolve<Tags>()._parent = this;
        }

        public bool Has(object tag)
        {
            Tags? scope = this;

            while (scope is not null)
            {
                if (scope._autofac.Tag == tag)
                {
                    return true;
                }

                scope = scope._parent;
            }

            return false;
        }
    }
}
