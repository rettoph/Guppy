using Guppy.EntityComponent;
using Guppy.EntityComponent.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Services
{
    internal sealed class EntityService : IEntityService, IDisposable
    {
        private IServiceProvider _provider;
        private ISetupProvider _setups;
        private List<IEntity> _list;

        public event OnEventDelegate<IEntityService, IEntity>? OnEntityAdded;
        public event OnEventDelegate<IEntityService, IEntity>? OnEntityRemoved;

        public EntityService(IServiceProvider provider, ISetupProvider setups)
        {
            _provider = provider;
            _setups = setups;
            _list = new List<IEntity>();
        }

        public void Dispose()
        {
            while (_list.Any())
            {
                this.TryRemove(_list.First());
            }
        }

        public bool TryAdd(IEntity entity)
        {
            if(_setups.TryCreate(_provider, entity))
            {
                _list.Add(entity);

                this.OnEntityAdded?.Invoke(this, entity);

                return true;
            }

            return false;
        }

        public TEntity Create<TEntity>()
            where TEntity : IEntity
        {
            TEntity entity = _provider.GetRequiredService<TEntity>();
           
            if(!this.TryAdd(entity))
            {
                throw new ArgumentException("Unable to add requested entity instance.", nameof(TEntity));
            }

            return entity;
        }

        public IEntity Create(Type type)
        {
            typeof(IEntity).ValidateAssignableFrom(type);

            IEntity? entity = _provider.GetRequiredService(type) as IEntity;

            if(entity is null || !this.TryAdd(entity))
            {
                throw new ArgumentException("Unable to add requested entity instance.", nameof(type));
            }

            return entity;
        }

        public bool TryRemove(IEntity entity)
        {
            if(!_list.Remove(entity))
            {
                return false;
            }

            if(_setups.TryDestroy(_provider, entity))
            {
                this.OnEntityRemoved?.Invoke(this, entity);

                return true;
            }

            return false;
        }

        public IEnumerator<IEntity> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
