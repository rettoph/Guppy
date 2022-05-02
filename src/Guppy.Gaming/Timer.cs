using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming
{
    public sealed class Timer : IDisposable
    {
        private double _time;
        private bool _enabled;
        private Queue<(Guid guid, Action<Guid, GameTime> action)> _queue;

        public readonly string Key;
        public readonly double Interval;

        public bool Enabled
        {
            get => _enabled;
            set => this.OnEnabledChanged!.InvokeIf(value != _enabled, this, ref _enabled, value);
        }

        public event OnEventDelegate<Timer, GameTime>? OnInterval;
        public event OnEventDelegate<Timer, bool>? OnEnabledChanged;
        public event OnEventDelegate<Timer>? OnDisposed;

        internal Timer(string key, double interval)
        {
            _queue = new Queue<(Guid guid, Action<Guid, GameTime> action)>();

            this.Key = key;
            this.Interval = interval;
        }

        public void Enqueue(Guid id, Action<Guid, GameTime> method)
        {
            _queue.Enqueue((id, method));
        }

        internal void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - _time > this.Interval)
            {
                while (_queue.TryDequeue(out (Guid guid, Action<Guid, GameTime> method) action))
                {
                    action.method(action.guid, gameTime);
                }

                this.OnInterval?.Invoke(this, gameTime);
                _time = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        public void Reset()
        {
            _time = 0;
        }
        public void Reset(bool enabled)
        {
            this.Enabled = enabled;
            this.Reset();
        }

        public void Dispose()
        {
            _queue.Clear();
            this.OnDisposed?.Invoke(this);
        }
    }
}
