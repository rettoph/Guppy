using Guppy.EntityComponent.Services;
using Guppy.EntityComponent.Services.Common;
using Guppy.Services.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
            set => this.OnSceneChanged.InvokeIf(value != _scene, this, ref _scene, value);
        }

        public event OnChangedEventDelegate<ISceneService, Scene?>? OnSceneChanged;

        public SceneService(IServiceProvider provider) : base(provider)
        {
        }

        protected override Guid GetId(Scene item)
        {
            return item.Id;
        }

        protected override TItem? Create<TItem>(IServiceProvider provider)
            where TItem : class
        {
            var scope = provider.CreateScope();
            var scene = base.Create<TItem>(scope.ServiceProvider);

            if(this.Scene is null && scene is not null)
            {
                this.Scene = scene;
            }

            return scene;
        }
    }
}
