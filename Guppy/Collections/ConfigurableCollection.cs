using Guppy.Factories;
using Guppy.Interfaces;
using Guppy.Utilities;
using Guppy.Utilities.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    public class ConfigurableCollection<TConfigurable> : OrderableCollection<TConfigurable>
        where TConfigurable : IConfigurable
    {
        private ConfigurableFactory<TConfigurable> _factory;

        #region Constructor
        public ConfigurableCollection(ConfigurableFactory<TConfigurable> factory, IServiceProvider provider) : base(provider)
        {
            _factory = factory;
        }
        #endregion

        #region Create Method
        public T Create<T>(Action<T> setup = null, Action<T> create = null)
            where T : class, TConfigurable
        {
            var entity = _factory.Build<T>(setup, create);
            this.Add(entity);
            return entity;
        }

        public T Create<T>(Type type, String handle, Action<T> setup = null, Action<T> create = null)
            where T : TConfigurable
        {
            ExceptionHelper.ValidateAssignableFrom<T>(type);

            var entity = _factory.Build<T>(type, handle, setup, create);
            this.Add(entity);
            return entity;
        }

        public T Create<T>(String handle, Action<T> setup = null, Action<T> create = null)
            where T : TConfigurable
        {
            return this.Create<T>(typeof(T), handle, setup, create);
        }

        public TConfigurable Create(String handle, Action<TConfigurable> setup = null, Action<TConfigurable> create = null)
        {
            return this.Create<TConfigurable>(handle, setup, create);
        }
        #endregion
    }
}
