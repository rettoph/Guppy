﻿using Guppy.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    internal sealed class GuppyProvider : IGuppyProvider
    {
        private IServiceProvider _provider;
        private IList<IScoped<IGuppy>> _guppies;

        public event OnEventDelegate<IGuppyProvider, IScoped<IGuppy>>? OnAdded;
        public event OnEventDelegate<IGuppyProvider, IScoped<IGuppy>>? OnRemoved;

        public GuppyProvider(IServiceProvider provider)
        {
            _guppies = new List<IScoped<IGuppy>>();
            _provider = provider;
        }

        IScoped<T> IGuppyProvider.Create<T>()
        {
            var guppy = _provider.GetRequiredService<IScoped<T>>();

            guppy.OnDispose += this.HandleGuppyDisposed;

            _guppies.Add(guppy);

            guppy.Instance.Initialize(guppy.Scope.ServiceProvider);

            this.OnAdded?.Invoke(this, guppy);

            return guppy;
        }

        private void HandleGuppyDisposed(IDisposable args)
        {
            if(args is IScoped<IGuppy> guppy)
            {
                guppy.OnDispose -= this.HandleGuppyDisposed;

                _guppies.Remove(guppy);

                this.OnRemoved?.Invoke(this, guppy);
            }
        }

        IEnumerable<IScoped<IGuppy>> IGuppyProvider.All()
        {
            return _guppies;
        }

        IEnumerable<IScoped<T>> IGuppyProvider.All<T>()
        {
            foreach(IScoped<IGuppy> guppy in _guppies)
            {
                if(guppy is IScoped<T> casted)
                {
                    yield return casted;
                }
            }
        }
    }
}