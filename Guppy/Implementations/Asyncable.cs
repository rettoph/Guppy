using Guppy.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Guppy.Implementations
{
    public abstract class Asyncable : Driven, IAsyncable
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

        #region IAsyncable Implementation
        public void TryStartAsync(Int32 delay = 16, Boolean draw = false)
        {
            if(this.RunningAsync)
            {
                this.logger.LogWarning($"Unable to start async loop. Loop already running.");
            }
            else
            {
                this.logger.LogInformation($"Starting async loop for {this.GetType().Name}({this.Id})...");

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
                this.logger.LogInformation($"Attempting to stop async loop for {this.GetType().Name}({this.Id})...");
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

            this.logger.LogInformation($"Closing async loop for {this.GetType().Name}({this.Id}).");
        }
    }
}
