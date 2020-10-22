using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Events.Delegates
{
    public delegate void OnEventDelegate<TSender>(TSender sender);
    public delegate void OnEventDelegate<TSender, TArgs>(TSender sender, TArgs args);

    public static class OnEventDelegateExtensions
    {
        public static void InvokeIfChanged<TSender, TArgs>(this OnEventDelegate<TSender, TArgs>  eventDelegate, Boolean changed, TSender sender, ref TArgs reference, TArgs value)
        {
            if (changed)
            {
                reference = value;
                eventDelegate?.Invoke(sender, reference);
            }
        }
    }
}
