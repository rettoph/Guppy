using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Providers
{
    internal sealed class TimerProvider : ITimerProvider
    {
        private Dictionary<string, Timer> _timers;
        private HashSet<Timer> _enabled;

        public TimerProvider()
        {
            _timers = new Dictionary<string, Timer>();
            _enabled = new HashSet<Timer>();

        }
        public Timer this[string key] => _timers[key];

        public Timer Create(string key, double interval, bool enabled)
        {
            var timer = new Timer(key, interval);
            _timers.Add(key, timer);
            timer.OnEnabledChanged += this.HandleTimerEnabledChanged;
            timer.OnDisposed += this.HandleTimerDisposed;
            timer.Enabled = enabled;

            return timer;
        }

        public Timer Get(string key)
        {
            return _timers[key];
        }

        public bool TryGet(string key, [MaybeNullWhen(false)] out Timer timer)
        {
            return _timers.TryGetValue(key, out timer);
        }

        void ITimerProvider.Update(GameTime gameTime)
        {
            foreach(Timer timer in _enabled)
            {
                timer.Update(gameTime);
            }
        }

        private void HandleTimerDisposed(Timer args)
        {
            _timers.Remove(args.Key);
            _enabled.Remove(args);
        }

        private void HandleTimerEnabledChanged(Timer sender, bool args)
        {
            if(args)
            {
                _enabled.Add(sender);
            }
            else
            {
                _enabled.Remove(sender);
            }
        }

        public IEnumerator<Timer> GetEnumerator()
        {
            return _timers.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
