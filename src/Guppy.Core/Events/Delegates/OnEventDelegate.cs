using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Events.Delegates
{
    public delegate void OnEventDelegate<TSender>(TSender sender);
    public delegate void OnEventDelegate<TSender, TArgs>(TSender sender, TArgs args);

    public static class OnEventDelegateExtensions
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
        public static void InvokeIfChanged<TSender, TArgs>(
            this OnEventDelegate<TSender, TArgs>  eventDelegate, 
            Boolean valid, 
            TSender sender, 
            ref TArgs reference, 
            TArgs value)
        {
            if (valid)
            {
                reference = value;
                eventDelegate?.Invoke(sender, reference);
            }
        }
    }
}
