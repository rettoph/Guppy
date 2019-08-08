using Guppy.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Enums;

namespace Guppy.Implementations
{
    public class Driver<TDriven> : Frameable, IDriver
        where TDriven : class, IDriven
    {
        protected TDriven parent { get; private set; }

        /// <summary>
        /// Update the objects parent.
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(IDriven parent)
        {
            if (this.InitializationStatus != InitializationStatus.NotInitialized)
                throw new Exception("Unable to set parent. Status is not NotInitialized.");

            this.parent = parent as TDriven;
        }
    }
}
