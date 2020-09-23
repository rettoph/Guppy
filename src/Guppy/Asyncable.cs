using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Guppy
{
    /// <summary>
    /// Represents a service than can self update (but doesnt neccessarily)
    /// </summary>
    public class Asyncable : Driven
    {
        private Boolean _draw;
        private Boolean _running;
        private DateTime _last;
        private DateTime _now;
        private DateTime _start;
        private GameTime _gameTime;
        private Thread _loop;

        protected override void Release()
        {
            base.Release();

            if (_running)
                this.TryStop();
        }

        public void TryStart(Boolean draw = false)
        {
            if (_running)
                throw new Exception("Unable to start Asyncable, already running.");

            this.Start(draw);
        }

        protected virtual void Start(Boolean draw)
        {
            _draw = true;
            
            _loop = new Thread(new ThreadStart(this.Loop));
            _loop.Start();
        }

        public void TryStop()
        {
            if (!_running)
                throw new Exception("Unable to stop Asyncable, not running.");

            this.Stop();
        }

        protected virtual void Stop()
        {
            _running = false;
        }

        private void Loop()
        {
            _running = true;
            _start = DateTime.Now;
            _now = DateTime.Now;

            while (_running)
            {
                Thread.Sleep(16);

                _last = _now;
                _now = DateTime.Now;

                _gameTime = new GameTime(_now - _start, _now - _last);

                this.TryUpdate(_gameTime);

                if (_draw)
                    this.TryDraw(_gameTime);
            }
        }
    }
}
