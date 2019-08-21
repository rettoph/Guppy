using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Implementations
{
    public class Driver : Driven
    {
        #region Protected Attributes
        protected Driven driven { get; private set; }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Update the internal parent
        /// </summary>
        /// <param name="driven"></param>
        internal void SetParent(Driven driven)
        {
            this.driven = driven;
        }
        #endregion
    }
}
