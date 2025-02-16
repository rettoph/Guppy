using System.Runtime.InteropServices;
using Guppy.Core.Common;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Services;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;
using Standart.Hash.xxHash;

namespace Guppy.Game.Common
{
    public abstract class Scene : IScene, IDisposable
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
        private bool _initialized;
        private bool _disposed;
        private readonly ActionSequenceGroup<DrawSequenceGroupEnum, GameTime> _drawActions;
        private readonly ActionSequenceGroup<UpdateSequenceGroupEnum, GameTime> _updateActions;
        private readonly IGuppyScope _scope;

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

        public Scene(IGuppyScope scope)
        {
            this._drawActions = new ActionSequenceGroup<DrawSequenceGroupEnum, GameTime>(true);
            this._updateActions = new ActionSequenceGroup<UpdateSequenceGroupEnum, GameTime>(false);
            this._initialized = false;

            this._scope = scope;

            this.Systems = null!;
            this.Id = Scene.CalculateId(this);
            this.Visible = true;
            this.Enabled = true;
        }

        void IScene.Initialize()
        {
            if (this._initialized == true)
            {
                return;
            }

            if (this._disposed == true)
            {
                return;
            }

            this._initialized = true;
            this.Initialize();
        }

        protected virtual void Initialize()
        {
            this.Systems = this._scope.Systems;
            this.InitializeSystems(this.Systems);

            this._drawActions.Add(this.Systems.GetAll());
            this._updateActions.Add(this.Systems.GetAll());
        }

        protected virtual void InitializeSystems(IScopedSystemService systemService)
        {
            // Call all system initializeation methods
            ActionSequenceGroup<InitializeSequenceGroupEnum>.Invoke(this.Systems.GetAll(), false);
        }

        void IScene.Deinitialize()
        {
            if (this._initialized == false)
            {
                return;
            }

            this._initialized = false;
            this.Deinitialize();
        }

        protected virtual void Deinitialize()
        {
            // Call all system deinitializeation methods
            Type initializeDelegate = typeof(Action<>).MakeGenericType(this.GetType());
            ActionSequenceGroup<DeinitializeSequenceGroupEnum>.Invoke(this.Systems.GetAll(), false);
        }

        public virtual void Draw(GameTime gameTime)
        {
            this._drawActions?.Invoke(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            this._updateActions.Invoke(gameTime);
        }

        public T Resolve<T>()
            where T : notnull
        {
            return this._scope.Resolve<T>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    if (this._initialized == true)
                    {
                        this._initialized = false;
                        this.Deinitialize();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                this._disposed = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Scene()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}