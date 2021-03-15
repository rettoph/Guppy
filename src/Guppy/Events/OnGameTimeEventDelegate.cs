using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Events
{
    public delegate void OnGameTimeEventDelegate<TSender>(TSender sender, GameTime gameTime);
    public delegate void OnGameTimeEventDelegate<TSender, TArgs>(TSender sender, GameTime gameTime, TArgs args);

    public static class OnGameTimeEventDelegateExtensions
    {
        /// <summary>
        /// Helper method to automatically invoke an event
        /// call if the value has changed.
        /// </summary>
        /// <typeparam name="TSender"></typeparam>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="eventDelegate"></param>
        /// <param name="valid">Whether or not the event should be invoked.</param>
        /// <param name="sender">The sender instance.</param>
        /// <param name="reference">A reference to be updated with the new value if valid.</param>
        /// <param name="value">The value to set the reference if valid.</param>
        public static Boolean InvokeIf<TSender, TArgs>(
            this OnGameTimeEventDelegate<TSender, TArgs> eventDelegate,
            Boolean valid,
            TSender sender,
            GameTime gameTime,
            ref TArgs reference,
            TArgs value)
        {
            if (valid)
            {
                reference = value;
                eventDelegate?.Invoke(sender, gameTime, reference);

                return true;
            }

            return false;
        }
    }
}
