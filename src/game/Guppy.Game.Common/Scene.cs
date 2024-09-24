using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;
using Standart.Hash.xxHash;
using System.Runtime.InteropServices;

namespace Guppy.Game.Common
{
    public abstract class Scene : IScene
    {
        private static Dictionary<Type, ushort> _count = new Dictionary<Type, ushort>();
        private static ulong CalculateId(Scene instance)
        {
            ref ushort count = ref CollectionsMarshal.GetValueRefOrAddDefault(_count, instance.GetType(), out bool exists);
            uint hash = xxHash32.ComputeHash(instance.GetType().AssemblyQualifiedName);

            return (ulong)hash << 32 | (ulong)count++;
        }

        private bool _enabled;
        private bool _visible;
        private ILifetimeScope _scope;
        private ActionSequenceGroup<DrawComponentSequenceGroup, GameTime> _drawComponentsActions;
        private ActionSequenceGroup<UpdateComponentSequenceGroup, GameTime> _updateComponentsActions;

        public event OnEventDelegate<IScene, bool>? OnEnabledChanged;
        public event OnEventDelegate<IScene, bool>? OnVisibleChanged;

        public ISceneComponent[] Components { get; private set; }

        public virtual string Name => this.GetType().Name;

        public ulong Id { get; }
        public bool Enabled
        {
            get => _enabled;
            set => this.OnEnabledChanged!.InvokeIf(_enabled != value, this, ref _enabled, value);
        }
        public bool Visible
        {
            get => _visible;
            set => this.OnVisibleChanged!.InvokeIf(_visible != value, this, ref _visible, value);
        }

        public Scene()
        {
            _scope = null!;
            _drawComponentsActions = new ActionSequenceGroup<DrawComponentSequenceGroup, GameTime>();
            _updateComponentsActions = new ActionSequenceGroup<UpdateComponentSequenceGroup, GameTime>();

            this.Components = Array.Empty<ISceneComponent>();


            this.Id = CalculateId(this);

            this.Visible = true;
            this.Enabled = true;
        }

        void IScene.Initialize(ILifetimeScope scope)
        {
            this.Initialize(scope);
        }

        protected virtual void Initialize(ILifetimeScope scope)
        {
            _scope = scope;

            this.Components = scope.Resolve<IFiltered<ISceneComponent>>().Sequence<ISceneComponent, InitializeComponentSequenceGroup>().ToArray();

            foreach (ISceneComponent component in this.Components)
            {
                component.Initialize();
            }

            _drawComponentsActions.Add(this.Components);
            _updateComponentsActions.Add(this.Components);
        }

        public virtual void Draw(GameTime gameTime)
        {
            _drawComponentsActions?.Invoke(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            _updateComponentsActions.Invoke(gameTime);
        }

        public T Resolve<T>()
            where T : notnull
        {
            return _scope.Resolve<T>();
        }
    }
}
