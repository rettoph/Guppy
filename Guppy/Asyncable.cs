using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Guppy
{
    /// <summary>
    /// Asyncable objects are frameable objects that can
    /// be ran asynchronously. This allows for multiple
    /// instances of them to run simulatiously. Mostly
    /// seen in multiplayer servers where there are several
    /// Scene instances running at the same time.
    /// </summary>
    public abstract class Asyncable : Driven
    {
        #region Private Fields
        private Int32 _delay;
        private Boolean _draw;
        private Thread _thread;
        #endregion

        #region Public Attributes
        public Boolean RunningAsync { get; private set; }
        #endregion

        #region Lifecycle Methods
        public override void Dispose()
        {
            base.Dispose();

            if (this.RunningAsync)
                this.TryStopAsync();
        }
        #endregion

        #region Helper Methods
        public void TryStartAsync(Int32 delay = 16, Boolean draw = false)
        {
            if (this.RunningAsync)
            {
                this.logger.LogWarning($"Unable to start async loop. Loop already running.");
            }
            else
            {
                this.logger.LogDebug($"Starting async loop for {this.GetType().Name}({this.Id})...");

                _delay = delay;
                _draw = draw;
                _thread = new Thread(new ThreadStart(this.loop));
                _thread.Start();
            }
        }

        public void TryStopAsync()
        {
            if (!this.RunningAsync)
            {
                this.logger.LogWarning($"Unable to start async loop. Loop already running.");
            }
            else
            {
                this.logger.LogDebug($"Attempting to stop async loop for {this.GetType().Name}({this.Id})...");
                this.RunningAsync = false;
            }
        }
        #endregion

        private void loop()
        {
            // Mark the loop as running...
            this.RunningAsync = true;

            DateTime now = DateTime.Now;
            DateTime start = DateTime.Now;
            DateTime last = DateTime.Now;
            GameTime time;

            while (this.RunningAsync)
            {
                Thread.Sleep(_delay);

                now = DateTime.Now;
                time = new GameTime(now.Subtract(start), now.Subtract(last));
                last = now;

                this.Update(time);

                if (_draw)
                    this.Draw(time);
            }

            this.logger.LogDebug($"Closing async loop for {this.GetType().Name}({this.Id}).");
        }
    }
}
