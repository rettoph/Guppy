namespace System
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
    public delegate bool ValidateEventDelegate<TSender, TArgs>(TSender sender, TArgs args);

    public static class ValidateEventDelegateExtensions
    {
        public static bool Validate<TSender, TArgs>(this ValidateEventDelegate<TSender, TArgs> validator, TSender sender, TArgs args, bool ifNull)
        {
            if (validator == default)
            {
                return ifNull;
            }
            else
            {
                foreach (ValidateEventDelegate<TSender, TArgs> v in validator.GetInvocationList().Cast<ValidateEventDelegate<TSender, TArgs>>())
                {
                    if (!v(sender, args))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}