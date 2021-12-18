using Guppy.Utilities;
using Guppy.Utilities.Threading;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Guppy
{
    /// <summary>
    /// Represents a service than can self update (but doesnt neccessarily)
    /// </summary>
    public class Asyncable : Frameable
    {
        private Boolean _draw;
        private CancellationTokenSource _cancelation;
        private Task _loop;

        /// <summary>
        /// Whether or not the current Asyncable instance is
        /// running in another thread.
        /// </summary>
        public Boolean Running => !_cancelation.IsCancellationRequested;

        protected override void Release()
        {
            base.Release();

            if (!_cancelation.IsCancellationRequested)
                this.TryStopAsync();
        }

        public virtual async Task TryStartAsync(Boolean draw = false, Int32 period = 16)
        {
            if (_cancelation?.IsCancellationRequested ?? false)
                throw new Exception("Unable to start Asyncable, already running.");

            await this.StartAsync(draw, period);
        }

        protected virtual async Task StartAsync(Boolean draw, Int32 period)
        {
            _cancelation = new CancellationTokenSource();

            _loop = TaskHelper.CreateLoop(this.Frame, period, _cancelation.Token);

            await _loop;
        }

        public async Task TryStopAsync()
        {
            if (_cancelation.IsCancellationRequested)
                throw new Exception("Unable to stop Asyncable, cancelation has already been requested.");

            await this.StopAsync();
        }

        protected virtual async Task StopAsync()
        {
            _cancelation.Cancel();

            await _loop;
        }

        private void Frame(GameTime gameTime)
        {
            this.TryUpdate(gameTime);


            if (_draw)
                this.TryDraw(gameTime);
        }
    }
}
