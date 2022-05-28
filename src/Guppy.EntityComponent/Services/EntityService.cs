using Guppy.EntityComponent;
using Guppy.EntityComponent.Providers;
using Guppy.Services.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Services
{
    internal sealed class EntityService : ListService<Guid, IEntity>, IEntityService, IDisposable
    {
        private ISetupProvider _setups;

        public EntityService(IServiceProvider provider, ISetupProvider setups) : base(provider)
        {
            _setups = setups;
        }

        public void Initialize()
        {
            _setups.Load();
        }

        public void Dispose()
        {
            while (this.items.Any())
            {
                this.TryRemove(this.items.First().Value);
            }
        }

        protected override Guid GetKey(IEntity item)
        {
            return item.Id;
        }

        protected override bool TryAdd(Guid key, IEntity item)
        {
            if (_setups.TryInitialize(item) && base.TryAdd(key, item))
            {
                item.OnDisposed += this.HandleItemDisposed;
                return true;
            }

            return false;
        }

        protected override bool TryRemove(Guid key, IEntity item)
        {
            if (!base.TryRemove(key, item))
            {
                return false;
            }

            if (!_setups.TryUninitialize(item))
            {
                return false;
            }

            item.OnDisposed -= this.HandleItemDisposed;

            return true;
        }

        private void HandleItemDisposed(IEntity entity)
        {
            this.TryRemove(entity);
        }
    }
}
