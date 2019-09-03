using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    /// <summary>
    /// Generic driver types will not require the IsDriven attribute,
    /// unless a specific priority is required.
    /// </summary>
    /// <typeparam name="TDriven"></typeparam>
    public class Driver<TDriven> : Driver
        where TDriven : Driven
    {
        #region Protected Fields
        protected TDriven driven { get; private set; }
        #endregion

        #region Constructor
        public Driver(TDriven driven) : base(driven)
        {
            this.driven = driven;
        }
        #endregion
    }
}
