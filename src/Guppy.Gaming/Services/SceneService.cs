using Guppy.EntityComponent.Services;
using Guppy.Services.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Services
{
    internal sealed class SceneService : ListService<Guid, Scene>, ISceneService
    {
        private Scene? _scene;

        public Scene? Scene
        {
            get => _scene;
            set => this.OnSceneChanged!.InvokeIf(value != _scene, this, ref _scene, value);
        }

        public event OnChangedEventDelegate<ISceneService, Scene?>? OnSceneChanged;

        public SceneService(IServiceProvider provider) : base(provider)
        {
        }

        protected override Guid GetKey(Scene item)
        {
            return item.Id;
        }

        protected override bool TryAdd(Guid key, Scene item)
        {
            if(base.TryAdd(key, item) && this.Scene is null)
            {
                this.Scene = item;
                return true;
            }

            return false;
        }

        protected override IServiceProvider GetFactoryServiceProvider(IServiceProvider provider)
        {
            return base.GetFactoryServiceProvider(provider).CreateScope().ServiceProvider;
        }
    }
}
