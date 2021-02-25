using Guppy.Contexts;
using Guppy.Events.Delegates;
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
        public event OnEventDelegate<IOrderable, Boolean> OnVisibleChanged;
        public event OnEventDelegate<IOrderable, Boolean> OnEnabledChanged;
        #endregion

        #region Properties
        Boolean Visible { get; set; }
        Boolean Enabled { get; set; }
        Int32 DrawOrder { get; set; }
        Int32 UpdateOrder { get; set; }
        #endregion

        #region Methods
        void SetContext(OrderableContext context);
        #endregion
    }
}
