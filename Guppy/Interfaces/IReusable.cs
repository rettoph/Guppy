using Guppy.Enums;
using Guppy.Utilities.Delegaters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    /// <summary>
    /// Simple interface defining a Guppy child.
    /// 
    /// Reusable objects should contain a working
    /// lifecycle, unique id, and can be pooled (and reused)
    /// </summary>
    public interface IReusable : IFrameable
    {
        #region Attributes
        EventDelegater Events { get; }
        #endregion

        #region Lifecycle Methods
        void TryCreate(IServiceProvider provider);
        #endregion

        #region Helper Methods
        void SetId(Guid id);
        void SetUpdateOrder(Int32 value);
        void SetDrawOrder(Int32 value);
        void SetVisible(Boolean value);
        void SetEnabled(Boolean value);
        #endregion
    }
}
