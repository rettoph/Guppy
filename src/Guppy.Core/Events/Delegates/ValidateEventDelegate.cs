using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Events.Delegates
{
    /// <summary>
    /// Multicast Predicate Delegate that will return false
    /// if anything invoked returns false.
    /// 
    /// Try using the Validate() extension method.
    /// </summary>
    /// <typeparam name="TSender"></typeparam>
    /// <typeparam name="TArgs"></typeparam>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public delegate Boolean ValidateEventDelegate<TSender, TArgs>(TSender sender, TArgs args);

    public static class ValidateEventDelegateExtensions
    {
        public static Boolean Validate<TSender, TArgs>(this ValidateEventDelegate<TSender, TArgs> validator, TSender sender, TArgs args)
        {
            foreach (ValidateEventDelegate<TSender, TArgs> v in validator.GetInvocationList())
                if (!v(sender, args))
                    return false;

            return true;
        }
    }
}
