using System.Runtime.InteropServices;
using Guppy.Core.Common;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Services;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;
using Standart.Hash.xxHash;

namespace Guppy.Game.Common
{
    public abstract class Scene : IScene
    {
        private static readonly Dictionary<Type, ushort> _count = [];
        private static ulong CalculateId(Scene instance)
        {
            ref ushort count = ref CollectionsMarshal.GetValueRefOrAddDefault(_count, instance.GetType(), out _);
            uint hash = xxHash32.ComputeHash(instance.GetType().AssemblyQualifiedName);

            return ((ulong)hash << 32) | count++;
        }

        private bool _enabled;
        private bool _visible;
        private IGuppyScope _scope;
        private readonly ActionSequenceGroup<DrawComponentSequenceGroupEnum, GameTime> _drawComponentsActions;
        private readonly ActionSequenceGroup<UpdateComponentSequenceGroupEnum, GameTime> _updateComponentsActions;

        public event OnEventDelegate<IScene, bool>? OnEnabledChanged;
        public event OnEventDelegate<IScene, bool>? OnVisibleChanged;

        public IScopedSystemService Systems { get; private set; }

        public virtual string Name => this.GetType().Name;

        public ulong Id { get; }
        public bool Enabled
        {
            get => this._enabled;
            set => this.OnEnabledChanged!.InvokeIf(this._enabled != value, this, ref this._enabled, value);
        }
        public bool Visible
        {
            get => this._visible;
            set => this.OnVisibleChanged!.InvokeIf(this._visible != value, this, ref this._visible, value);
        }

        public Scene()
        {
            this._scope = null!;
            this._drawComponentsActions = new ActionSequenceGroup<DrawComponentSequenceGroupEnum, GameTime>(true);
            this._updateComponentsActions = new ActionSequenceGroup<UpdateComponentSequenceGroupEnum, GameTime>(false);

            this.Systems = null!;


            this.Id = CalculateId(this);

            this.Visible = true;
            this.Enabled = true;
        }

        void IScene.Initialize(IGuppyScope scope)
        {
            this.Initialize(scope);
        }

        protected virtual void Initialize(IGuppyScope scope)
        {
            this._scope = scope;

            this.Systems = scope.ResolveService<IScopedSystemService>();

            Type initializeDelegate = typeof(Action<>).MakeGenericType(this.GetType());
            DelegateSequenceGroup<InitializeSystemSequenceGroupEnum>.Invoke(this.Systems, initializeDelegate, false, [this]);

            this._drawComponentsActions.Add(this.Systems);
            this._updateComponentsActions.Add(this.Systems);
        }

        public virtual void Draw(GameTime gameTime)
        {
            this._drawComponentsActions?.Invoke(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            this._updateComponentsActions.Invoke(gameTime);
        }

        public T Resolve<T>()
            where T : notnull
        {
            return this._scope.ResolveService<T>();
        }
    }
}