﻿using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Autofac;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Engine.Common.Providers;
using System.Collections;

namespace Guppy.Engine.Providers
{
    internal sealed class GuppyProvider : IGuppyProvider
    {
        private ILifetimeScope _scope;
        private List<IGuppy> _guppies;
        private Dictionary<IGuppy, ILifetimeScope> _scopes;
        private bool _initialized;

        public ILifetimeScope Scope => _scope;

        public event OnEventDelegate<IGuppyProvider, IGuppy>? OnGuppyCreated;
        public event OnEventDelegate<IGuppyProvider, IGuppy>? OnGuppyDestroyed;

        public GuppyProvider(ILifetimeScope scope)
        {
            _guppies = new List<IGuppy>();
            _scope = scope;
            _scopes = new Dictionary<IGuppy, ILifetimeScope>();
        }

        public void Initialize()
        {
            if (_initialized == true)
            {
                return;
            }

            var components = this.Scope.Resolve<IEnumerable<IGlobalComponent>>().Sequence(InitializeSequence.Initialize).ToArray();
            foreach (IGlobalComponent component in components)
            {
                component.Initialize(components);
            }

            _initialized = true;
        }

        public T Create<T>(Action<ContainerBuilder>? guppyBuilder)
            where T : class, IGuppy
        {
            this.Initialize();

            var scope = _scope.BeginLifetimeScope(LifetimeScopeTags.GuppyScope, builder =>
            {
                builder.RegisterType<T>().AsSelf().AsImplementedInterfaces().SingleInstance();
                guppyBuilder?.Invoke(builder);
            });
            var guppy = scope.Resolve<T>();

            this.Configure(guppy, scope);

            return guppy;
        }

        public IGuppy Create(Type guppyType, Action<ContainerBuilder>? guppyBuilder)
        {
            ThrowIf.Type.IsNotAssignableFrom<IGuppy>(guppyType);

            this.Initialize();

            var scope = _scope.BeginLifetimeScope(LifetimeScopeTags.GuppyScope, builder =>
            {
                builder.RegisterType(guppyType).AsSelf().AsImplementedInterfaces().SingleInstance();
                guppyBuilder?.Invoke(builder);
            });
            var guppy = scope.Resolve(guppyType) as IGuppy;

            if (guppy is null)
            {
                throw new NotImplementedException();
            }

            this.Configure(guppy, scope);

            return guppy;
        }

        public void Destroy(IGuppy guppy)
        {
            if (_scopes.Remove(guppy, out var scope))
            {
                scope.Dispose();
            }

            _guppies.Remove(guppy);
            this.OnGuppyDestroyed?.Invoke(this, guppy);
        }

        private void Configure(IGuppy guppy, ILifetimeScope scope)
        {
            guppy.Initialize(scope);
            _scopes.Add(guppy, scope);
            _guppies.Add(guppy);

            this.OnGuppyCreated?.Invoke(this, guppy);
        }

        public IEnumerator<IGuppy> GetEnumerator()
        {
            return _guppies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Dispose()
        {
            _scope.Dispose();

            while (_guppies.Any())
            {
                this.Destroy(_guppies.First());
            }
        }
    }
}
