using Guppy.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IOrderable : IFrameable
    {
        #region Events
        public event OnEventDelegate<IOrderable, Int32> OnDrawOrderChanged;
        public event OnEventDelegate<IOrderable, Int32> OnUpdateOrderChanged;
        #endregion

        #region Properties
        Int32 DrawOrder { get; set; }
        Int32 UpdateOrder { get; set; }
        #endregion

        #region Methods
        void SetContext(OrderableContext context);
        #endregion
    }
}
